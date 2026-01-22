using TaskManagement.Models;

namespace TaskManagement.Repositories
{
    public interface ITaskRepository
    {
        IEnumerable<TaskItem> GetTasks(int userId, bool? isCompleted);
        TaskItem GetById(int id, int userId);
        void Add(TaskItem task);
        void Update(TaskItem task);
        void Delete(int id);  // , int UserId
    }
}
