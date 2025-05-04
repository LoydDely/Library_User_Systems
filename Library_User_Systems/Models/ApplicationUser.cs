using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Library_User_Systems.Models
{
	/// <summary>
	/// This class represents a application user with its extended properties.
	/// </summary>
	public class ApplicationUser : IdentityUser
	{
		/// <summary>
		/// This gets or sets the full name of the user.
		/// </summary>
		[Required]
		public string FullName { get; set; }

		/// <summary>
		/// Get or sets the user's phone number.
		/// </summary>
		public string PhoneNumber { get; set; }

		/// <summary>
		/// This gets or sets the type of users. Student, Staff, or Admin
		/// </summary>
		[Required]
		public string UserType { get; set; } 

		/// <summary>
		/// This returns a value whether or not the user is active.
		/// Not used right. May add front end view at later date.
		/// </summary>
		public bool IsActive { get; set; } = true;

		/// <summary>
		/// This gets the borrowing limit per user.
		/// </summary>
		public int BorrowingLimit => UserType switch
		{
			"Student" => 3,
			"Staff" => 5,
			"Admin" => 5,
			_ => 1
		};
	}
}
