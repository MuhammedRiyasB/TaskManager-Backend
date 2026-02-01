using TaskManager.Modules.Tasks.DTOs;

namespace TaskManager.Modules.Tasks.Service;

public interface ITaskService
{
    Task<List<TaskResponse>> GetAllAsync();
    Task<TaskResponse?> GetByIdAsync(int id);
    Task<TaskResponse> CreateAsync(CreateTaskRequest request);
    Task<bool> UpdateAsync(int id, UpdateTaskRequest request);
    Task<bool> ToggleAsync(int id);
    Task<bool> DeleteAsync(int id);
}
