using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManagement.Models;
using TaskManagement.Repositories;

namespace TaskManagement.Pages.Tasks
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly ITaskRepository _repo;

        public EditModel(ITaskRepository repo)
        {
            _repo = repo;
        }

        [BindProperty]
        public TaskItem Task { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var task = _repo.GetById(id, userId);
            if (task == null)
                return Forbid(); 

            Task = task;
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

           
            var existingTask = _repo.GetById(Task.TaskId, userId);
            if (existingTask == null)
                return Forbid();

            existingTask.Title = Task.Title;
            existingTask.Description = Task.Description;
            existingTask.IsCompleted = Task.IsCompleted;

            _repo.Update(existingTask);

            return RedirectToPage("Index");
        }
    }

}
