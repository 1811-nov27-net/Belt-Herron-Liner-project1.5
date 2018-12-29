﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClassLibrary;
using DataAccess;

namespace CharacterSheet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        public IRepo Repo { get; set; }

        public UserController(IRepo repo)
        {
            Repo = repo;
        }

        // GET: api/User
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return Repo.UserList();
        }

        // GET: api/User/5
        [HttpGet("{id}", Name = "Get")]
        public User Get(int id)
        {
            return Repo.UserDetails(id);
        }

        // POST: api/User
        [HttpPost]
        public void Post([FromBody] User user)
        {
            Repo.CreateUser(user);
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Repo.DeleteUser(id);
        }
    }
}
