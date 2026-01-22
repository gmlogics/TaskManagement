using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManagement.Models;
using TaskManagement.Repositories;

namespace TaskManagement.Pages.Tasks
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ITaskRepository _repo;

        public CreateModel(ITaskRepository repo)
        {
            _repo = repo;
        }

        [BindProperty]
        public TaskItem Task { get; set; } = new();

        //public IActionResult OnPost()
        //{
        //    if (!ModelState.IsValid)
        //        return Page();

        //    Task.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        //    _repo.Add(Task);

        //    return RedirectToPage("Index");
        //}

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }

                return Page();
            }

            Task.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            _repo.Add(Task);
            return RedirectToPage("Index");
        }
    }
}
