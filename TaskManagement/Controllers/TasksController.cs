using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagement.Data;
using TaskManagement.Models;

[Authorize(AuthenticationSchemes = "Bearer")]
[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly TaskDbContext _context;

    public TasksController(TaskDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Get()
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        return Ok(_context.Tasks.Where(t => t.UserId == userId).ToList());
    }

    [HttpPost]
    public IActionResult Create(TaskItem task)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        task.UserId = userId;
        _context.Tasks.Add(task);
        _context.SaveChanges();

        return Ok(task);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, TaskItem input)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var task = _context.Tasks.FirstOrDefault(t => t.TaskId == id && t.UserId == userId);
        if (task == null) return NotFound();

        task.Title = input.Title;
        task.Description = input.Description;
        task.IsCompleted = input.IsCompleted;

        _context.SaveChanges();
        return Ok(task);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var task = _context.Tasks.FirstOrDefault(t => t.TaskId == id && t.UserId == userId);
        if (task == null) return NotFound();

        _context.Tasks.Remove(task);
        _context.SaveChanges();
        return NoContent();
    }
}
