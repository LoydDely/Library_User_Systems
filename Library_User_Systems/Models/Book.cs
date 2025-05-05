using System.ComponentModel.DataAnnotations;

namespace Library_User_Systems.Models
{
    /// <summary>
    /// Represents a book in the library system.
    /// </summary>
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Author { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// True if the book is available to borrow.
        /// </summary>
        public bool IsAvailable { get; set; } = true;
    }
}
