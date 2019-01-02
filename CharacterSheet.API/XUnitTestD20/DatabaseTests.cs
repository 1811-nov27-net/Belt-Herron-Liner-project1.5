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
        [InlineData(0, "Test Campaign")]
        [InlineData(2, "Test Campaign2")]
        [InlineData(0, "")]
        [InlineData(0, null)]
        public async void CreateCampaignWorks(int id, string name)
        {
            var options = new DbContextOptionsBuilder<Data.D20CharacterDatabaseContext>()
                .UseInMemoryDatabase("create_campaign_test").Options;
            using (var db = new Data.D20CharacterDatabaseContext(options))
            {
                Lib.IRepo sut = new Data.D20Repo(db);
                Lib.Campaign camp = new Lib.Campaign();
                if (await db.Campaign.FirstOrDefaultAsync(c => c.CampaignId == 1) == null)
                {
                    camp.CampID = 0;
                    camp.Name = "No Campaign";
                    camp.Characters = new List<Lib.Character>();
                    camp.GMs = new List<Lib.User>();
                    db.Campaign.Add(camp);
                    db.SaveChanges();
                }
                camp.CampID = id;
                camp.Name = name;
                camp.Characters = new List<Lib.Character>();
                camp.GMs = new List<Lib.User>();
                sut.CreateCampaign(camp);
                Data.Campaign testCamp = await db.Campaign.FirstOrDefaultAsync(c => c.CampaignName == name);
                

                if (id == 0)
                {
                    bool actual = (testCamp != null && testCamp.CampaignName == name);
                    Assert.True(actual);
                }
                else
                {
                    bool actual = (testCamp != null && testCamp.CampaignId == id);
                    Assert.False(actual);
                }
            }
        }
        [Theory]
        [InlineData(1, "Test Character")]
        [InlineData(2, "Test Character2")]
        [InlineData(0, "Test Character3")]
        [InlineData(3, "")]
        [InlineData(4, null)]
        public async void CreateCharacterWorks(int charId, string name)
        {
            var options = new DbContextOptionsBuilder<Data.D20CharacterDatabaseContext>()
                .UseInMemoryDatabase("create_character_test").Options;
            using (var db = new Data.D20CharacterDatabaseContext(options))
            {
                Lib.IRepo sut = new Data.D20Repo(db);
                Lib.Character testChar = new Lib.Character();
                if (await db.Characters.FirstOrDefaultAsync(c => c.CharacterId == 1) == null)
                {
                    testChar.CharID = 0;
                    testChar.Name = "No Character";
                    testChar.CampID = 1;
                    testChar.UserID = 1;
                    db.Characters.Add(testChar);
                    db.SaveChanges();
                }
                testChar.CharID = charId;
                testChar.Name = name;
                testChar.CampID = 1;
                testChar.UserID = 1;
                testChar.CharID = sut.CreateCharacter(testChar);
                Data.Characters test = await db.Characters.FirstOrDefaultAsync(c => c.CharacterName == name);
                bool actual = (test != null && test.CharacterId == testChar.CharID);

                Assert.True(actual);
            }
        }
        [Theory]
        [InlineData(0, "Test User")]
        [InlineData(1, "Test User2")]
        [InlineData(0, "")]
        [InlineData(0, null)]
        public async void CreateUserWorks(int id, string name)
        {
            var options = new DbContextOptionsBuilder<Data.D20CharacterDatabaseContext>()
                .UseInMemoryDatabase("create_user_test").Options;
            using (var db = new Data.D20CharacterDatabaseContext(options))
            {
                Lib.IRepo sut = new Data.D20Repo(db);
                Lib.User user = new Lib.User();
                if(await db.Gamer.FirstOrDefaultAsync(g => g.GamerId == 1) == null)
                {
                    user.UserID = 0;
                    user.Username = "NPC";
                    user.Characters = new List<Lib.Character>();
                    user.MyCampaigns = new List<Lib.Campaign>();
                    db.Gamer.Add(user);
                    db.SaveChanges();
                }
                user.UserID = id;
                user.Username = name;
                user.Characters = new List<Lib.Character>();
                user.MyCampaigns = new List<Lib.Campaign>();
                sut.CreateUser(user);

                Data.Gamer testUser = await db.Gamer.FirstOrDefaultAsync(u => u.UserName == name);

                if (id == 0)
                {
                    bool actual = (testUser != null && testUser.UserName == name);
                    Assert.True(actual);
                }
                else
                {
                    bool actual = (testUser != null && testUser.GamerId == id);
                    Assert.False(actual);
                }
            }
        }
        [Theory]
        [InlineData(0, "Test Campaign")]
        [InlineData(0, "")]
        [InlineData(0, null)]
        public async void DeleteCampaignWorks(int id, string name)
        {
            var options = new DbContextOptionsBuilder<Data.D20CharacterDatabaseContext>()
                .UseInMemoryDatabase("delete_campaign_test").Options;
            using (var db = new Data.D20CharacterDatabaseContext(options))
            {
                Lib.IRepo sut = new Data.D20Repo(db);
                Lib.Campaign camp = new Lib.Campaign();
                if (await db.Campaign.FirstOrDefaultAsync(c => c.CampaignId == 1) == null)
                {
                    camp.CampID = 0;
                    camp.Name = "No Campaign";
                    camp.Characters = new List<Lib.Character>();
                    camp.GMs = new List<Lib.User>();
                    db.Campaign.Add(camp);
                    db.SaveChanges();
                }
                camp.CampID = id;
                camp.Name = name;
                camp.Characters = new List<Lib.Character>();
                camp.GMs = new List<Lib.User>();
                sut.CreateCampaign(camp);
                Data.Campaign testCamp = await db.Campaign.FirstOrDefaultAsync(c => c.CampaignName == name);
                bool actual = (testCamp != null && testCamp.CampaignName == name);

                Assert.True(actual);

                sut.DeleteCamp(testCamp.CampaignId);
                testCamp = await db.Campaign.FirstOrDefaultAsync(c => c.CampaignName == name);
                actual = testCamp == null;
                Assert.True(actual);
            }
        }
        [Theory]
        [InlineData(1, "Test Character")]
        [InlineData(2, "Test Character2")]
        [InlineData(0, "Test Character3")]
        [InlineData(3, "")]
        [InlineData(4, null)]
        public async void DeleteCharacterWorks(int charId, string name)
        {
            var options = new DbContextOptionsBuilder<Data.D20CharacterDatabaseContext>()
                .UseInMemoryDatabase("delete_character_test").Options;
            using (var db = new Data.D20CharacterDatabaseContext(options))
            {
                Lib.IRepo sut = new Data.D20Repo(db);
                Lib.Character testChar = new Lib.Character();
                if (await db.Characters.FirstOrDefaultAsync(c => c.CharacterId == 1) == null)
                {
                    testChar.CharID = 0;
                    testChar.Name = "No Character";
                    testChar.CampID = 1;
                    testChar.UserID = 1;
                    db.Characters.Add(testChar);
                    db.SaveChanges();
                }
                testChar.CharID = charId;
                testChar.Name = name;
                testChar.CampID = 1;
                testChar.UserID = 1;
                sut.CreateCharacter(testChar);
                Data.Characters test = await db.Characters.FirstOrDefaultAsync(c => c.CharacterName == name);
                bool actual = (test != null && test.CampaignId == 1);

                Assert.True(actual);

                testChar = test;
                sut.DeleteChar(testChar.CharID);
                test = await db.Characters.FirstOrDefaultAsync(c => c.CharacterId == testChar.CharID);
                actual = test == null;

                Assert.True(actual);
            }
        }
        [Theory]
        [InlineData(0, "Test User")]
        [InlineData(0, "")]
        [InlineData(0, null)]
        public async void DeleteUserWorks(int id, string name)
        {
            var options = new DbContextOptionsBuilder<Data.D20CharacterDatabaseContext>()
                .UseInMemoryDatabase("delete_user_test").Options;
            using (var db = new Data.D20CharacterDatabaseContext(options))
            {
                Lib.IRepo sut = new Data.D20Repo(db);
                Lib.User user = new Lib.User();
                if (await db.Gamer.FirstOrDefaultAsync(g => g.GamerId == 1) == null)
                {
                    user.UserID = 0;
                    user.Username = "NPC";
                    user.Characters = new List<Lib.Character>();
                    user.MyCampaigns = new List<Lib.Campaign>();
                    db.Gamer.Add(user);
                    db.SaveChanges();
                }
                user.UserID = id;
                user.Username = name;
                user.Characters = new List<Lib.Character>();
                user.MyCampaigns = new List<Lib.Campaign>();
                sut.CreateUser(user);

                Data.Gamer testUser = await db.Gamer.FirstOrDefaultAsync(u => u.UserName == name);
                bool actual = (testUser != null && testUser.GamerId != 0);

                Assert.True(actual);

                sut.DeleteUser(testUser.GamerId);
                testUser = await db.Gamer.FirstOrDefaultAsync(u => u.UserName == name);
                actual = testUser == null;
                Assert.True(actual);
            }
        }
        [Theory]
        [InlineData(0, "Test Campaign")]
        [InlineData(0, "")]
        [InlineData(0, null)]
        public async void UpdateCampaignWorks(int id, string name)
        {
            var options = new DbContextOptionsBuilder<Data.D20CharacterDatabaseContext>()
                .UseInMemoryDatabase("update_campaign_test").Options;
            using (var db = new Data.D20CharacterDatabaseContext(options))
            {
                Lib.IRepo sut = new Data.D20Repo(db);
                Lib.Campaign camp = new Lib.Campaign();
                if (await db.Campaign.FirstOrDefaultAsync(c => c.CampaignId == 1) == null)
                {
                    camp.CampID = 0;
                    camp.Name = "No Campaign";
                    camp.Characters = new List<Lib.Character>();
                    camp.GMs = new List<Lib.User>();
                    db.Campaign.Add(camp);
                    db.SaveChanges();
                }
                camp.CampID = id;
                camp.Name = name;
                camp.Characters = new List<Lib.Character>();
                camp.GMs = new List<Lib.User>();
                sut.CreateCampaign(camp);
                Data.Campaign testCamp = await db.Campaign.FirstOrDefaultAsync(c => c.CampaignName == name);
                bool actual = (testCamp != null && testCamp.CampaignId != 0);

                Assert.True(actual);

                camp.Name = "Update Test";
                camp.CampID = testCamp.CampaignId;
                sut.UpdateCamp(camp);
                testCamp = await db.Campaign.FirstOrDefaultAsync(c => c.CampaignId == testCamp.CampaignId);
                actual = testCamp.CampaignName == "Update Test";
                Assert.True(actual);
            }
        }
        [Theory]
        [InlineData(1, "Test Character")]
        [InlineData(2, "Test Character2")]
        [InlineData(0, "Test Character3")]
        [InlineData(3, "")]
        [InlineData(4, null)]
        public async void UpdateCharacterWorks(int charId, string name)
        {
            var options = new DbContextOptionsBuilder<Data.D20CharacterDatabaseContext>()
                .UseInMemoryDatabase("update_character_test").Options;
            using (var db = new Data.D20CharacterDatabaseContext(options))
            {
                Lib.IRepo sut = new Data.D20Repo(db);
                Lib.Character testChar = new Lib.Character();
                if (await db.Characters.FirstOrDefaultAsync(c => c.CharacterId == 1) == null)
                {
                    testChar.CharID = 0;
                    testChar.Name = "No Character";
                    testChar.CampID = 1;
                    testChar.UserID = 1;
                    db.Characters.Add(testChar);
                    db.SaveChanges();
                }
                testChar.CharID = charId;
                testChar.Name = name;
                testChar.CampID = 1;
                testChar.UserID = 1;
                sut.CreateCharacter(testChar);
                Data.Characters test = await db.Characters.FirstOrDefaultAsync(c => c.CharacterName == name);
                bool actual = (test != null && test.CampaignId == 1);

                Assert.True(actual);

                testChar = test;
                testChar.Name = "Update Test";
                sut.UpdateCharacter(testChar);
                test = await db.Characters.FirstOrDefaultAsync(c => c.CharacterId == testChar.CharID);
                actual = test.CharacterName == "Update Test";

                Assert.True(actual);
            }
        }
        [Theory]
        [InlineData(0, "Test User")]
        [InlineData(0, "")]
        [InlineData(0, null)]
        public async void UpdateUserWorks(int id, string name)
        {
            var options = new DbContextOptionsBuilder<Data.D20CharacterDatabaseContext>()
                .UseInMemoryDatabase("update_user_test").Options;
            using (var db = new Data.D20CharacterDatabaseContext(options))
            {
                Lib.IRepo sut = new Data.D20Repo(db);
                Lib.User user = new Lib.User();
                if (await db.Gamer.FirstOrDefaultAsync(g => g.GamerId == 1) == null)
                {
                    user.UserID = 0;
                    user.Username = "NPC";
                    user.Characters = new List<Lib.Character>();
                    user.MyCampaigns = new List<Lib.Campaign>();
                    db.Gamer.Add(user);
                    db.SaveChanges();
                }
                user.UserID = id;
                user.Username = name;
                user.Characters = new List<Lib.Character>();
                user.MyCampaigns = new List<Lib.Campaign>();
                sut.CreateUser(user);

                Data.Gamer testUser = await db.Gamer.FirstOrDefaultAsync(u => u.UserName == name);
                bool actual = (testUser != null && testUser.GamerId != 0);

                Assert.True(actual);
                user = testUser;
                user.Username = "Update Test";
                sut.UpdateUser(user);
                testUser = await db.Gamer.FirstOrDefaultAsync(u => u.GamerId == user.UserID);
                actual = testUser.UserName == "Update Test";
            }
        }
        [Theory]
        [InlineData(0, "Test Campaign")]
        [InlineData(0, "")]
        [InlineData(0, null)]
        public async void CampaignDetailsWorks(int id, string name)
        {
            var options = new DbContextOptionsBuilder<Data.D20CharacterDatabaseContext>()
                .UseInMemoryDatabase("campaign_details_test").Options;
            using (var db = new Data.D20CharacterDatabaseContext(options))
            {
                Lib.IRepo sut = new Data.D20Repo(db);
                Lib.Campaign camp = new Lib.Campaign();
                if (await db.Campaign.FirstOrDefaultAsync(c => c.CampaignId == 1) == null)
                {
                    camp.CampID = 0;
                    camp.Name = "No Campaign";
                    camp.Characters = new List<Lib.Character>();
                    camp.GMs = new List<Lib.User>();
                    db.Campaign.Add(camp);
                    db.SaveChanges();
                }
                camp.CampID = id;
                camp.Name = name;
                camp.Characters = new List<Lib.Character>();
                camp.GMs = new List<Lib.User>();
                sut.CreateCampaign(camp);
                Data.Campaign testCamp = await db.Campaign.FirstOrDefaultAsync(c => c.CampaignName == name);
                bool actual = (testCamp != null && testCamp.CampaignName == name);

                Assert.True(actual);

                camp = sut.CampDetails(testCamp.CampaignId);
                actual = camp.Name == name;

                Assert.True(actual);
            }
        }
        [Theory]
        [InlineData(1, "Test Character")]
        [InlineData(3, "")]
        [InlineData(4, null)]
        public async void CharDetailsWorks(int charId, string name)
        {
            var options = new DbContextOptionsBuilder<Data.D20CharacterDatabaseContext>()
                .UseInMemoryDatabase("character_details_test").Options;
            using (var db = new Data.D20CharacterDatabaseContext(options))
            {
                Lib.IRepo sut = new Data.D20Repo(db);
                Lib.Character testChar = new Lib.Character();
                if (await db.Characters.FirstOrDefaultAsync(c => c.CharacterId == 1) == null)
                {
                    testChar.CharID = 0;
                    testChar.Name = "No Character";
                    testChar.CampID = 1;
                    testChar.UserID = 1;
                    db.Characters.Add(testChar);
                    db.SaveChanges();
                }
                testChar.CharID = charId;
                testChar.Name = name;
                testChar.CampID = 1;
                testChar.UserID = 1;
                sut.CreateCharacter(testChar);
                Data.Characters test = await db.Characters.FirstOrDefaultAsync(c => c.CharacterName == name);
                bool actual = (test != null && test.CampaignId == 1);

                Assert.True(actual);

                testChar = sut.CharDetails(test.CharacterId);
                actual = testChar.Name == name;

                Assert.True(actual);
            }
        }
        [Theory]
        [InlineData(0, "Test User")]
        [InlineData(0, "")]
        [InlineData(0, null)]
        public async void UserDetailsWorks(int id, string name)
        {
            var options = new DbContextOptionsBuilder<Data.D20CharacterDatabaseContext>()
                .UseInMemoryDatabase("create_user_test").Options;
            using (var db = new Data.D20CharacterDatabaseContext(options))
            {
                Lib.IRepo sut = new Data.D20Repo(db);
                Lib.User user = new Lib.User();
                if (await db.Gamer.FirstOrDefaultAsync(g => g.GamerId == 1) == null)
                {
                    user.UserID = 0;
                    user.Username = "NPC";
                    user.Characters = new List<Lib.Character>();
                    user.MyCampaigns = new List<Lib.Campaign>();
                    db.Gamer.Add(user);
                    db.SaveChanges();
                }
                user.UserID = id;
                user.Username = name;
                user.Characters = new List<Lib.Character>();
                user.MyCampaigns = new List<Lib.Campaign>();
                sut.CreateUser(user);

                Data.Gamer testUser = await db.Gamer.FirstOrDefaultAsync(u => u.UserName == name);
                bool actual = (testUser != null && testUser.UserName == name);
                Assert.True(actual);

                user = sut.UserDetails(testUser.GamerId);
                actual = user.Username == name;

                Assert.True(actual);
            }
        }
    [Fact]
    public async void AddGMWorks()
        {
            var options = new DbContextOptionsBuilder<Data.D20CharacterDatabaseContext>()
                .UseInMemoryDatabase("add_gm_test").Options;
            using (var db = new Data.D20CharacterDatabaseContext(options))
            {
                Lib.IRepo sut = new Data.D20Repo(db);
                Lib.User user = new Lib.User();
                if (await db.Gamer.FirstOrDefaultAsync(g => g.GamerId == 1) == null)
                {
                    user.UserID = 0;
                    user.Username = "NPC";
                    user.Characters = new List<Lib.Character>();
                    user.MyCampaigns = new List<Lib.Campaign>();
                    db.Gamer.Add(user);
                    db.SaveChanges();
                }
                user.UserID = 0;
                user.Username = "Test User";
                user.Characters = new List<Lib.Character>();
                user.MyCampaigns = new List<Lib.Campaign>();
                sut.CreateUser(user);
                Lib.Campaign camp = new Lib.Campaign();
                if (await db.Campaign.FirstOrDefaultAsync(c => c.CampaignId == 1) == null)
                {
                    camp.CampID = 0;
                    camp.Name = "No Campaign";
                    camp.Characters = new List<Lib.Character>();
                    camp.GMs = new List<Lib.User>();
                    db.Campaign.Add(camp);
                    db.SaveChanges();
                }
                camp.CampID = 0;
                camp.Name = "Test Campaign";
                camp.Characters = new List<Lib.Character>();
                camp.GMs = new List<Lib.User>();
                sut.CreateCampaign(camp);
                user = await db.Gamer.FirstOrDefaultAsync(u => u.UserName == "Test User");
                camp = await db.Campaign.FirstOrDefaultAsync(c => c.CampaignName == "Test Campaign");

                sut.AddGM(camp.CampID, user.UserID);

                var result = await db.Gmjunction.FirstOrDefaultAsync(g => g.CampaignId == camp.CampID);
                bool actual = result.Gmid == user.UserID;

                Assert.True(actual);
            }
        }
        [Fact]
        public async void CampaignListWorks()
        {
            var options = new DbContextOptionsBuilder<Data.D20CharacterDatabaseContext>()
                .UseInMemoryDatabase("campaign_list_test").Options;
            using (var db = new Data.D20CharacterDatabaseContext(options))
            {
                Lib.IRepo sut = new Data.D20Repo(db);
                Lib.Campaign camp = new Lib.Campaign();
                if (await db.Campaign.FirstOrDefaultAsync(c => c.CampaignId == 1) == null)
                {
                    camp.CampID = 0;
                    camp.Name = "No Campaign";
                    camp.Characters = new List<Lib.Character>();
                    camp.GMs = new List<Lib.User>();
                    db.Campaign.Add(camp);
                    db.SaveChanges();
                }
                camp.CampID = 0;
                camp.Name = "Test Campaign";
                camp.Characters = new List<Lib.Character>();
                camp.GMs = new List<Lib.User>();
                sut.CreateCampaign(camp);
                Lib.Campaign camp2 = new Lib.Campaign();
                camp2.CampID = 0;
                camp2.Name = "Test Campaign2";
                camp2.Characters = new List<Lib.Character>();
                camp2.GMs = new List<Lib.User>();
                sut.CreateCampaign(camp2);

                Data.Campaign testCamp = await db.Campaign.FirstOrDefaultAsync(c => c.CampaignName == camp.Name);
                Data.Campaign testCamp2 = await db.Campaign.FirstOrDefaultAsync(c => c.CampaignName == camp2.Name);
                bool actual = (testCamp != null && testCamp.CampaignName == camp.Name && testCamp2 != null && testCamp2.CampaignName == camp2.Name);
                Assert.True(actual);

                IList<Lib.Campaign> campList = (List<Lib.Campaign>)sut.CampList();

                actual = (campList.Count == 3 && campList[1].Name == camp.Name && campList[2].Name == camp2.Name);

                Assert.True(actual);
            }
        }
        [Fact]
        public async void CharacterListWorks()
        {
            var options = new DbContextOptionsBuilder<Data.D20CharacterDatabaseContext>()
                .UseInMemoryDatabase("character_list_test").Options;
            using (var db = new Data.D20CharacterDatabaseContext(options))
            {
                Lib.IRepo sut = new Data.D20Repo(db);
                Lib.Character testChar = new Lib.Character();
                if (await db.Characters.FirstOrDefaultAsync(c => c.CharacterId == 1) == null)
                {
                    testChar.CharID = 0;
                    testChar.Name = "No Character";
                    testChar.CampID = 1;
                    testChar.UserID = 1;
                    db.Characters.Add(testChar);
                    db.SaveChanges();
                }
                testChar.CharID = 0;
                testChar.Name = "Test Character";
                testChar.CampID = 1;
                testChar.UserID = 1;
                sut.CreateCharacter(testChar);
                Lib.Character testChar2 = new Lib.Character();
                testChar2.CharID = 0;
                testChar2.Name = "Test Character2";
                testChar2.CampID = 1;
                testChar2.UserID = 1;
                sut.CreateCharacter(testChar2);
                Data.Characters test = await db.Characters.FirstOrDefaultAsync(c => c.CharacterName == testChar.Name);
                Data.Characters test2 = await db.Characters.FirstOrDefaultAsync(c => c.CharacterName == testChar2.Name);
                bool actual = (test != null && test.CampaignId == 1 && test2 != null && test2.CampaignId == 1);

                Assert.True(actual);

                IList<Lib.Character> list = (IList<Lib.Character>)sut.CharacterList();
                actual = (list.Count == 3 && list[1].Name == test.CharacterName && list[2].Name == test2.CharacterName);

                Assert.True(actual);
            }
        }
        [Fact]
        public async void UserListWorks()
        {
            var options = new DbContextOptionsBuilder<Data.D20CharacterDatabaseContext>()
                .UseInMemoryDatabase("user_list_test").Options;
            using (var db = new Data.D20CharacterDatabaseContext(options))
            {
                Lib.IRepo sut = new Data.D20Repo(db);
                Lib.User user = new Lib.User();
                if (await db.Gamer.FirstOrDefaultAsync(g => g.GamerId == 1) == null)
                {
                    user.UserID = 0;
                    user.Username = "NPC";
                    user.Characters = new List<Lib.Character>();
                    user.MyCampaigns = new List<Lib.Campaign>();
                    db.Gamer.Add(user);
                    db.SaveChanges();
                }
                user.UserID = 0;
                user.Username = "Test User";
                user.Characters = new List<Lib.Character>();
                user.MyCampaigns = new List<Lib.Campaign>();
                sut.CreateUser(user);
                Lib.User user2 = new Lib.User();
                user2.UserID = 0;
                user2.Username = "Test User2";
                user2.Characters = new List<Lib.Character>();
                user2.MyCampaigns = new List<Lib.Campaign>();
                sut.CreateUser(user2);

                Data.Gamer testUser = await db.Gamer.FirstOrDefaultAsync(u => u.UserName == user.Username);
                Data.Gamer testUser2 = await db.Gamer.FirstOrDefaultAsync(u => u.UserName == user2.Username);

                bool actual = (testUser != null && testUser.UserName == user.Username && testUser2 != null && testUser2.UserName == user2.Username);
                Assert.True(actual);

                IList<Lib.User> list = (IList<Lib.User>)sut.UserList();
                actual = (list.Count == 3 && list[1].Username == user.Username && list[2].Username == user2.Username);

                Assert.True(actual);
            }
        }
        [Fact]
        public async void CharacterListByCampWorks()
        {
            var options = new DbContextOptionsBuilder<Data.D20CharacterDatabaseContext>()
                .UseInMemoryDatabase("character_list_by_camp_test").Options;
            using (var db = new Data.D20CharacterDatabaseContext(options))
            {
                Lib.IRepo sut = new Data.D20Repo(db);
                Lib.Character testChar = new Lib.Character();
                if (await db.Characters.FirstOrDefaultAsync(c => c.CharacterId == 1) == null)
                {
                    testChar.CharID = 0;
                    testChar.Name = "No Character";
                    testChar.CampID = 1;
                    testChar.UserID = 1;
                    db.Characters.Add(testChar);
                    db.SaveChanges();
                }
                testChar.CharID = 0;
                testChar.Name = "Test Character";
                testChar.CampID = 1;
                testChar.UserID = 1;
                sut.CreateCharacter(testChar);
                Lib.Character testChar2 = new Lib.Character();
                testChar2.CharID = 0;
                testChar2.Name = "Test Character2";
                testChar2.CampID = 2;
                testChar2.UserID = 1;
                sut.CreateCharacter(testChar2);
                Data.Characters test = await db.Characters.FirstOrDefaultAsync(c => c.CharacterName == testChar.Name);
                Data.Characters test2 = await db.Characters.FirstOrDefaultAsync(c => c.CharacterName == testChar2.Name);
                bool actual = (test != null && test.CampaignId == 1 && test2 != null && test2.CampaignId == 2);

                Assert.True(actual);

                IList<Lib.Character> list = (IList<Lib.Character>)sut.CharacterListByCamp(2);
                actual = (list.Count == 1 && list[0].Name == test2.CharacterName);

                Assert.True(actual);
            }
        }
        [Fact]
        public async void CharacterListByUserWorks()
        {
            var options = new DbContextOptionsBuilder<Data.D20CharacterDatabaseContext>()
                .UseInMemoryDatabase("character_list_by_user_test").Options;
            using (var db = new Data.D20CharacterDatabaseContext(options))
            {
                Lib.IRepo sut = new Data.D20Repo(db);
                Lib.Character testChar = new Lib.Character();
                if (await db.Characters.FirstOrDefaultAsync(c => c.CharacterId == 1) == null)
                {
                    testChar.CharID = 0;
                    testChar.Name = "No Character";
                    testChar.CampID = 1;
                    testChar.UserID = 1;
                    db.Characters.Add(testChar);
                    db.SaveChanges();
                }
                testChar.CharID = 0;
                testChar.Name = "Test Character";
                testChar.CampID = 1;
                testChar.UserID = 1;
                sut.CreateCharacter(testChar);
                Lib.Character testChar2 = new Lib.Character();
                testChar2.CharID = 0;
                testChar2.Name = "Test Character2";
                testChar2.CampID = 1;
                testChar2.UserID = 2;
                sut.CreateCharacter(testChar2);
                Data.Characters test = await db.Characters.FirstOrDefaultAsync(c => c.CharacterName == testChar.Name);
                Data.Characters test2 = await db.Characters.FirstOrDefaultAsync(c => c.CharacterName == testChar2.Name);
                bool actual = (test != null && test.GamerId == 1 && test2 != null && test2.GamerId == 2);

                Assert.True(actual);

                IList<Lib.Character> list = (IList<Lib.Character>)sut.CharacterListByUser(2);
                actual = (list.Count == 1 && list[0].Name == test2.CharacterName);

                Assert.True(actual);
            }
        }
        [Fact]
        public async void JoinCampWorks()
        {
            var options = new DbContextOptionsBuilder<Data.D20CharacterDatabaseContext>()
                .UseInMemoryDatabase("join_campaign_test").Options;
            using (var db = new Data.D20CharacterDatabaseContext(options))
            {
                Lib.IRepo sut = new Data.D20Repo(db);
                Lib.Campaign camp = new Lib.Campaign();
                if (await db.Campaign.FirstOrDefaultAsync(c => c.CampaignId == 1) == null)
                {
                    camp.CampID = 0;
                    camp.Name = "No Campaign";
                    camp.Characters = new List<Lib.Character>();
                    camp.GMs = new List<Lib.User>();
                    db.Campaign.Add(camp);
                    db.SaveChanges();
                }
                camp.CampID = 0;
                camp.Name = "Test Campaign";
                camp.Characters = new List<Lib.Character>();
                camp.GMs = new List<Lib.User>();
                sut.CreateCampaign(camp);
                Data.Campaign testCamp = await db.Campaign.FirstOrDefaultAsync(c => c.CampaignName == camp.Name);

                Lib.Character testChar = new Lib.Character();
                if (await db.Characters.FirstOrDefaultAsync(c => c.CharacterId == 1) == null)
                {
                    testChar.CharID = 0;
                    testChar.Name = "No Character";
                    testChar.CampID = 1;
                    testChar.UserID = 1;
                    db.Characters.Add(testChar);
                    db.SaveChanges();
                }
                testChar.CharID = 0;
                testChar.Name = "Test Character";
                testChar.CampID = 0;
                testChar.UserID = 1;
                sut.CreateCharacter(testChar);
                Data.Characters test = await db.Characters.FirstOrDefaultAsync(c => c.CharacterName == testChar.Name);

                bool actual = (testCamp != null && testCamp.CampaignName == camp.Name && test != null && test.CampaignId != testCamp.CampaignId);
                Assert.True(actual);

                sut.JoinCamp(testCamp.CampaignId, test.CharacterId);
                test = await db.Characters.FirstOrDefaultAsync(c => c.CharacterName == testChar.Name);
                actual = (test.CampaignId == testCamp.CampaignId);

                Assert.True(actual);
            }
        }
        [Fact]
        public async void RemoveCharFromCampWorks()
        {
            var options = new DbContextOptionsBuilder<Data.D20CharacterDatabaseContext>()
                .UseInMemoryDatabase("remove_char_from_campaign_test").Options;
            using (var db = new Data.D20CharacterDatabaseContext(options))
            {
                Lib.IRepo sut = new Data.D20Repo(db);
                Lib.Campaign camp = new Lib.Campaign();
                if (await db.Campaign.FirstOrDefaultAsync(c => c.CampaignId == 1) == null)
                {
                    camp.CampID = 0;
                    camp.Name = "No Campaign";
                    camp.Characters = new List<Lib.Character>();
                    camp.GMs = new List<Lib.User>();
                    db.Campaign.Add(camp);
                    db.SaveChanges();
                }

                Lib.Campaign camp2 = new Lib.Campaign();
                camp2.CampID = 0;
                camp2.Name = "Test Campaign";
                camp2.Characters = new List<Lib.Character>();
                camp2.GMs = new List<Lib.User>();
                sut.CreateCampaign(camp2);
                Data.Campaign testCamp2 = await db.Campaign.FirstOrDefaultAsync(c => c.CampaignName == camp2.Name);

                Lib.Character testChar = new Lib.Character();
                if (await db.Characters.FirstOrDefaultAsync(c => c.CharacterId == 1) == null)
                {
                    testChar.CharID = 0;
                    testChar.Name = "No Character";
                    testChar.CampID = 1;
                    testChar.UserID = 1;
                    db.Characters.Add(testChar);
                    db.SaveChanges();
                }
                testChar.CharID = 0;
                testChar.Name = "Test Character";
                testChar.CampID = 0;
                testChar.UserID = 1;
                sut.CreateCharacter(testChar);
                Data.Characters test = await db.Characters.FirstOrDefaultAsync(c => c.CharacterName == testChar.Name);

                bool actual = (testCamp2 != null && testCamp2.CampaignName == camp2.Name && test != null && test.CampaignId != testCamp2.CampaignId);
                Assert.True(actual);

                sut.JoinCamp(testCamp2.CampaignId, test.CharacterId);
                test = await db.Characters.FirstOrDefaultAsync(c => c.CharacterName == testChar.Name);
                actual = (test.CampaignId == testCamp2.CampaignId);

                Assert.True(actual);

                sut.RemoveCharFromCamp(testCamp2.CampaignId, test.CharacterId);
                test = await db.Characters.FirstOrDefaultAsync(c => c.CharacterName == testChar.Name);
                actual = test.CampaignId != testCamp2.CampaignId;

                Assert.True(actual);
            }
        }
    }
}
