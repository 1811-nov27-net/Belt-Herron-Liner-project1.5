﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using ClassLibrary;
using DataAccess;
using CharacterSheet.API.Controllers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;



namespace XUnitTestD20
{
    public class UserControllerTests
    {
        private Mock<SignInManager<IdentityUser>> _mockSignInManager;
        private Mock<IdentityDbContext> _db;

        [Fact]
        public void UserIndexTest()
        {

            var data = new List<User>
            {
                new User
                {
                    UserID = 1,
                    Username = "matt"
                }


            };

            var mockRepo = new Mock<IRepo>();
            mockRepo
                .Setup(repo => repo.UserList())
                .Returns(data);
            mockRepo
                .Setup(repo => repo.UserDetails(1))
                .Returns(data[0]);

            mockRepo
                .Setup(repo => repo.CreateUser(It.IsAny<User>()));

            var controller = new UserController(_mockSignInManager.Object, _db.Object, mockRepo.Object);

            ActionResult<IEnumerable<User>> result = controller.GetAsync();

            //ViewResult viewResult = Assert.IsAssignableFrom<ViewResult>(result);

            var users = Assert.IsAssignableFrom<ActionResult<IEnumerable<User>>>(result);
            var userList = users.Value.ToList();

            Assert.Equal(data.Count, userList.Count);
            for (int i = 0; i < data.Count; i++)
            {
                Assert.Equal(data[i].UserID, userList[i].UserID);
            }



        }
    }
}
