using Library_User_Systems.Models;
using Library_User_Systems.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Library_User_Systems.Services
{
    /// <summary>
    /// This class is used for user retrieval, updating, and deletion. 
    /// </summary>
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        /// <summary>
        /// This initializes a new instance of the class. 
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="dbContext"></param>
        public UserService(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        /// <summary>
        /// This method gets all of the users. 
        /// </summary>
        /// <returns></returns>
        public List<ApplicationUser> GetAllUsers()
        {
            return _userManager.Users.ToList();
        }

        /// <summary>
        /// This gets a user by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ApplicationUser GetUserById(string id)
        {
            return _userManager.Users.FirstOrDefault(u => u.Id == id);
        }

        /// <summary>
        /// This updates a users information and role. 
        /// </summary>
        /// <param name="updatedUser"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task UpdateUser(ApplicationUser updatedUser)
        {
            var existingUser = await _userManager.FindByIdAsync(updatedUser.Id);
            if (existingUser == null)
            {
                throw new Exception("User not found.");
            }

            
            existingUser.FullName = updatedUser.FullName;
            existingUser.Email = updatedUser.Email;
            existingUser.PhoneNumber = updatedUser.PhoneNumber;
            existingUser.UserType = updatedUser.UserType;
            existingUser.IsActive = updatedUser.IsActive;
            existingUser.UserName = updatedUser.Email;

            
            var currentRoles = await _userManager.GetRolesAsync(existingUser);
            if (currentRoles.Any())
            {
                await _userManager.RemoveFromRolesAsync(existingUser, currentRoles);
            }
            await _userManager.AddToRoleAsync(existingUser, updatedUser.UserType);

            await _userManager.UpdateAsync(existingUser);
        }

        /// <summary>
        /// This deletes a user by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);

                if (!result.Succeeded)
                {
                    throw new Exception("Failed to delete the user.");
                }
            }
        }
    }
}