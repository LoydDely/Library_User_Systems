using Library_User_Systems.Controllers;
using Library_User_Systems.Models;
using Library_User_Systems.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Xunit;


namespace Library_User_Systems.Tests
{
    /// <summary>
    /// This class contains unit tests for the UserController.
    /// </summary>
    public class UserControllerTests
    {
        /// <summary>
        /// This is a fake/dummy of IUserService for testing.
        /// </summary>
        private class FakeUserService : IUserService
        {
            public List<ApplicationUser> GetAllUsers() => new List<ApplicationUser>();
            public ApplicationUser GetUserById(string id) => null;
            public Task UpdateUser(ApplicationUser user) => Task.CompletedTask;
            public Task DeleteUserAsync(string id) => Task.CompletedTask;
        }

        /// <summary>
        /// This checks to see if Manage action returns a ViewResult.
        /// </summary>
        [Fact]
        public void Manage_ShouldReturnViewResult()
        {
            var service = new FakeUserService();
            var controller = new UsersController(service, null);

            // Act
            var result = controller.Manage();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        /// <summary>
        /// This verifies the Create (GET) action returns a ViewResult.
        /// </summary>
        [Fact]
        public void Create_Get_ShouldReturnViewResult()
        {
            // Arrange
            var controller = new UsersController(null, null);

            // Act
            var result = controller.Create();

            // Assert
            Assert.IsType<ViewResult>(result);
        }
    }


}