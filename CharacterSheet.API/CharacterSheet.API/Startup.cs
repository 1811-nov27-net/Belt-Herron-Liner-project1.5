using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary;
using DataAccess;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CharacterSheet.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddScoped<IRepo, D20Repo>();

            services.AddDbContext<D20CharacterDatabaseContext>(optionsBuilder => optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DBString")));

            services.AddDbContext<IdentityDbContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("UserApiAuthorization")));

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
                {
                    options.Password.RequiredLength = 25;
                    options.Password.RequiredUniqueChars = 1;
                    options.Password.RequireDigit = false;
                }).AddEntityFrameworkStores<IdentityDbContext>();

            services.ConfigureApplicationCookie(options =>
                {
                    options.Cookie.Name = "UserApiAuthorization";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(120);
                    options.Events = new CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = ctx =>
                        {
                            ctx.Response.StatusCode = 401;
                            return Task.FromResult(0);
                        },
                        OnRedirectToAccessDenied = ctx =>
                        {
                            ctx.Response.StatusCode = 403;
                            return Task.FromResult(0);
                        }
                    };
                });

            services.AddAuthentication();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
