using System;
using Xunit;
using Lib = ClassLibrary;
using Data = DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XUnitTestD20
{
    public class DatabaseTests
    {
        [Theory]
        [InlineData(1, "Test Campaign")]
        [InlineData(2, "Test Campaign")]
        //[InlineData(0, "Test Campaign")]
        [InlineData(3, "")]
        [InlineData(4, null)]
        public async void CreateCampaignWorks(int id, string name)
        {
            var options = new DbContextOptionsBuilder<Data.D20CharacterDatabaseContext>()
                .UseInMemoryDatabase("create_campaign_test").Options;
            using (var db = new Data.D20CharacterDatabaseContext(options))
            {
                Data.IRepo sut = new Data.D20Repo(db);
                Lib.Campaign camp = new Lib.Campaign();
                camp.CampID = id;
                camp.Name = name;
                camp.Characters = new List<Lib.Character>();
                camp.GMs = new List<Lib.User>();
                sut.CreateCampaign(camp);
                Data.Campaign testCamp = await db.Campaign.FirstOrDefaultAsync(c => c.CampaignId == id);
                bool actual = (testCamp != null && testCamp.CampaignName == name);

                Assert.True(actual);
            }
        }
        //[Theory]
        //[InlineData(1, "Test Character")]
        //[InlineData(2, "Test Character")]
        //[InlineData(0, "Test Character")]
        //[InlineData(3, "")]
        //[InlineData(4, null)]
        //public async void CreateCharacterWorks(int charId, string name)
        //{
        //    var options = new DbContextOptionsBuilder<Data.D20CharacterDatabaseContext>()
        //        .UseInMemoryDatabase("create_character_test").Options;
        //    using (var db = new Data.D20CharacterDatabaseContext(options))
        //    {
        //        Data.IRepo sut = new Data.D20Repo(db);
        //        Lib.Character testChar = new Lib.Character();
        //        testChar.CharID = charId;
        //        testChar.Name = name;
        //        testChar.CampID = 1;
        //        testChar.UserID = 1;
        //        sut.CreateCharacter(testChar);
        //        Data.Characters test = await db.Characters.FirstOrDefaultAsync(c => c.CharacterId == charId);
        //        bool actual = (test != null && test.CampaignId == 1);

        //        Assert.True(actual);
        //    }
        //}
        [Theory]
        [InlineData(1, "Test User")]
        [InlineData(2, "Test User")]
        //[InlineData(0, "Test User")]
        [InlineData(3, "")]
        [InlineData(4, null)]
        public async void CreateUserWorks(int id, string name)
        {
            var options = new DbContextOptionsBuilder<Data.D20CharacterDatabaseContext>()
                .UseInMemoryDatabase("create_user_test").Options;
            using (var db = new Data.D20CharacterDatabaseContext(options))
            {
                Data.IRepo sut = new Data.D20Repo(db);
                Lib.User user = new Lib.User();
                user.UserID = id;
                user.Username = name;
                user.Characters = new List<Lib.Character>();
                user.MyCampaigns = new List<Lib.Campaign>();
                sut.CreateUser(user);

                Data.Gamer testUser = await db.Gamer.FirstOrDefaultAsync(u => u.GamerId == id);
                bool actual = (testUser != null && testUser.UserName == name);

                Assert.True(actual);
            }
        }
        
    }
}
