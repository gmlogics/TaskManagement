using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManagement.Models;
using TaskManagement.Repositories;

namespace TaskManagement.Pages.Tasks
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ITaskRepository _repo;

        public IndexModel(ITaskRepository repo)
        {
            _repo = repo;
        }

        public List<TaskItem> Tasks { get; set; }

        public void OnGet(bool? status)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            Tasks = _repo.GetTasks(userId, status).ToList();
        }
    }
}
