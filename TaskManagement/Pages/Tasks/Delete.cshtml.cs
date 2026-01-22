using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManagement.Repositories;

namespace TaskManagement.Pages.Tasks
{

    

    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly ITaskRepository _repo;

        public DeleteModel(ITaskRepository repo)
        {
            _repo = repo;
        }

        public IActionResult OnPost(int id)
        {
            _repo.Delete(id);
            return RedirectToPage("Index");
        }
    }

}
