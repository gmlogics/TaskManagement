using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TaskManagement.Pages.Account
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnPost()
        {
            // Delete JWT cookie
            Response.Cookies.Delete("jwt");

            return RedirectToPage("/Account/Login");
        }
    }
}
