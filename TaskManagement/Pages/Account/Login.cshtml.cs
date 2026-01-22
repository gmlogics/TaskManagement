using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using TaskManagement.Data;

namespace TaskManagement.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly TaskDbContext _context;
        private readonly IConfiguration _config;

        public LoginModel(TaskDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = _context.Users.SingleOrDefault(u => u.Email == Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(Password, user.PasswordWithHash))
            {
                ErrorMessage = "Invalid credentials";
                return Page();
            }

            // ✅ Create claims
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
        new Claim(ClaimTypes.Email, user.Email)
    };

            var identity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            // ✅ THIS CREATES AUTH COOKIE (MOST IMPORTANT LINE)
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal);

            return RedirectToPage("/Tasks/Index");
        }

        //public IActionResult OnPost()
        //{
        //    var user = _context.Users.SingleOrDefault(u => u.Email == Email);

        //    if (user == null || !BCrypt.Net.BCrypt.Verify(Password, user.PasswordWithHash))
        //    {
        //        ErrorMessage = "Invalid credentials";
        //        return Page();
        //    }

        //    var claims = new[]
        //    {
        //    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
        //    new Claim(ClaimTypes.Email, user.Email)
        //};

        //    var key = new SymmetricSecurityKey(
        //        Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

        //    var token = new JwtSecurityToken(
        //        issuer: _config["Jwt:Issuer"],
        //        audience: _config["Jwt:Audience"],
        //        claims: claims,
        //        expires: DateTime.Now.AddHours(1),
        //        signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        //    );

        //    var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        //    Response.Cookies.Append("jwt", tokenValue);

        //    return RedirectToPage("/Tasks/Index");
        //}
    }
}
