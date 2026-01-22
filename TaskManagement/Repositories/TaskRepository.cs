using TaskManagement.Data;
using TaskManagement.Models;

namespace TaskManagement.Repositories
{
    public class TaskRepository:ITaskRepository
    {
        private readonly TaskDbContext _context;

        public TaskRepository(TaskDbContext context)
        {
            _context = context;
        }

        public IEnumerable<TaskItem> GetTasks(int userId, bool? isCompleted)
        {
            var query = _context.Tasks.Where(t => t.UserId == userId);

            if (isCompleted.HasValue)
                query = query.Where(t => t.IsCompleted == isCompleted);

            return query.ToList();
        }

        public TaskItem? GetById(int taskId, int userId)
        {
            return _context.Tasks
                .FirstOrDefault(t => t.TaskId == taskId && t.UserId == userId);
        }

        public void Add(TaskItem task)
        {
            task.CreatedAt = DateTime.Now;
            task.UpdatedAt = DateTime.Now;
            task.IsActive = true;

            _context.Tasks.Add(task);
            _context.SaveChanges();
        }

        public void Update(TaskItem task)
        {
            task.UpdatedAt = DateTime.Now;
            _context.Tasks.Update(task);
            _context.SaveChanges();
        }

        public void Delete(int id) // , int UserId
        {
            var task = _context.Tasks.Find(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                _context.SaveChanges();
            }
        }
    }
}
