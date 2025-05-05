using Library_User_Systems.Data;
using Library_User_Systems.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Library_User_Systems.Controllers
{
    [Authorize]
    public class BorrowingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BorrowingController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Borrowing
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var records = await _context.BorrowRecords
                .Where(r => r.UserId == userId)
                .Include(r => r.Book)  // ✅ now works!
                .ToListAsync();

            return View(records);
        }

        // GET: /Borrowing/Borrow
        public IActionResult Borrow()
        {
            var availableBooks = _context.Books
                .Where(b => b.IsAvailable)
                .ToList();

            ViewBag.BookList = new SelectList(availableBooks, "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Borrow(BorrowRecord record)
        {
	        record.UserId = _userManager.GetUserId(User);
	        ModelState.Remove("UserId");

	        if (!record.BookId.HasValue || record.BookId == 0)
	        {
		        ModelState.AddModelError("BookId", "You must select a book.");
	        }

	        // ✅ GET current user and active borrow count
	        var user = await _userManager.GetUserAsync(User);
	        var activeBorrows = await _context.BorrowRecords
		        .CountAsync(r => r.UserId == user.Id);

	        // ✅ ENFORCE borrow limit
	        if (activeBorrows >= user.BorrowingLimit)
	        {
		        ModelState.AddModelError("", $"You have reached your borrow limit of {user.BorrowingLimit} books.");
	        }

	        if (ModelState.IsValid)
	        {
		        record.BorrowDate = DateTime.Now;
		        record.DueDate = record.BorrowDate.AddDays(14);

		        var book = await _context.Books.FindAsync(record.BookId);
		        if (book != null)
		        {
			        book.IsAvailable = false;
			        _context.BorrowRecords.Add(record);
			        await _context.SaveChangesAsync();
			        return RedirectToAction(nameof(Index));
		        }

		        ModelState.AddModelError("", "Selected book not found.");
	        }

	        ViewBag.BookList = new SelectList(_context.Books.Where(b => b.IsAvailable), "Id", "Title");
	        return View(record);
        }


		// POST: /Borrowing/Return/5
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Return(int id)
        {
            var record = await _context.BorrowRecords.FindAsync(id);
            if (record == null) return NotFound();

            var book = await _context.Books.FindAsync(record.BookId);
            if (book != null)
            {
                book.IsAvailable = true;
                _context.BorrowRecords.Remove(record);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
