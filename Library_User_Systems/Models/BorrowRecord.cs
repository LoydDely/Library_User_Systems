using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library_User_Systems.Models
{
	/// <summary>
	/// Represents a record of a book borrowing transaction.
	/// </summary>
	public class BorrowRecord
	{
		[Key]
		public int Id { get; set; }

		public int? BookId { get; set; }

		[ForeignKey("BookId")]
		public Book? Book { get; set; }

		// Set server-side in the controller, not required in form
		public string UserId { get; set; }

		public DateTime BorrowDate { get; set; }

		public DateTime DueDate { get; set; }
	}
}