using Library_User_Systems.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Library_User_Systems.Data
{
	/// <summary>
	/// Application DB context that integrates ASP.NET Identity and app-specific entities.
	/// </summary>
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<Book> Books { get; set; }
		public DbSet<BorrowRecord> BorrowRecords { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			// Optional: Customize relationship behavior
			builder.Entity<BorrowRecord>()
				.HasOne(br => br.Book)
				.WithMany() // no navigation on Book
				.HasForeignKey(br => br.BookId)
				.OnDelete(DeleteBehavior.Restrict); // Prevent cascade cycles
		}
	}
}