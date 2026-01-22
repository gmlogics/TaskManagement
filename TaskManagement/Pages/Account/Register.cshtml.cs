using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManagement.Data;
using TaskManagement.Models;

namespace TaskManagement.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly TaskDbContext _context;

        public RegisterModel(TaskDbContext context)
        {
            _context = context;
        }

        [BindProperty] public string FullName { get; set; }
        [BindProperty] public string Email { get; set; }
        [BindProperty] public string Password { get; set; }

        public string ErrorMessage { get; set; }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var exists = _context.Users.Any(u => u.Email == Email);
            if (exists)
            {
                ModelState.AddModelError("", "Email already registered");
                return Page();
            }

            var user = new User
            {
                FullName = FullName,
                Email = Email,
                PasswordWithHash = BCrypt.Net.BCrypt.HashPassword(Password),
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToPage("/Account/Login");
        }
        //public IActionResult OnPost()
        //{
        //    if (_context.Users.Any(u => u.Email == Email))
        //    {
        //        ErrorMessage = "Email already exists";
        //        return Page();
        //    }

        //    var user = new User
        //    {
        //        FullName = FullName,
        //        Email = Email,
        //        PasswordWithHash = BCrypt.Net.BCrypt.HashPassword(Password)
        //    };

        //    _context.Users.Add(user);
        //    _context.SaveChanges();

        //    return RedirectToPage("Login");
        //}
    }
}
