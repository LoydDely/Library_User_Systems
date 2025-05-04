using Library_User_Systems.Models;
using Library_User_Systems.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Library_User_Systems.Controllers
{
    /// <summary>
    /// This class handles user management such as creating ,editing, and deleting users from the database.
    /// This page can only be accessed by the admin and staff.
    /// </summary>
    [Authorize(Roles = "Admin,Staff")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// This initializes the class.
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="userManager"></param>
        public UsersController(IUserService userService, UserManager<ApplicationUser> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }

        /// <summary>
        /// This displays a list of all the users on the website.
        /// </summary>
        /// <returns></returns>
        public IActionResult Manage()
        {
            var users = _userService.GetAllUsers();
            return View(users);
        }

        /// <summary>
        /// This displays the edit form for a user.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Edit(string id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        /// <summary>
        /// This handles the post request to update a user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationUser user)
        {
            if (!ModelState.IsValid)
                return View(user);

            await _userService.UpdateUser(user);
            return RedirectToAction("Manage");
        }

        /// <summary>
        /// This handles deleting a user by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            await _userService.DeleteUserAsync(id);
            return RedirectToAction("Manage");
        }

        /// <summary>
        /// This displays the form to create a new user.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View(new ApplicationUser());
        }

        /// <summary>
        /// This handles the post request ot create a new user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(ApplicationUser user)
        {
            var password = Request.Form["Password"];
            var confirm = Request.Form["ConfirmPassword"];

            if (string.IsNullOrWhiteSpace(password) || password != confirm)
            {
                ModelState.AddModelError("Password", "Passwords do not match.");
                return View(user);
            }

            if (!ModelState.IsValid)
            {
                return View(user);
            }

            user.UserName = user.Email;
            user.EmailConfirmed = true;

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, user.UserType);
                return RedirectToAction("Manage");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(user);
        }
    }
}
