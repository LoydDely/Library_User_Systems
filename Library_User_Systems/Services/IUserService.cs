using Library_User_Systems.Models;
using System.Collections.Generic;

namespace Library_User_Systems.Services
{
	/// <summary>
	/// Interface for user-related operations.
	/// </summary>
	public interface IUserService
	{
		/// <summary>
		/// List to get all of the users. 
		/// </summary>
		/// <returns></returns>
		List<ApplicationUser> GetAllUsers();
		/// <summary>
		/// This gets a user by there id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		ApplicationUser GetUserById(string id);
		/// <summary>
		/// This updates a user / user information.
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
        Task UpdateUser(ApplicationUser user);
		/// <summary>
		/// This deletes a user from the database by userid.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
        Task DeleteUserAsync(string id);
    }
}