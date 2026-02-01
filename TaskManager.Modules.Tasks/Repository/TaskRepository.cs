using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskManager.Modules.Tasks.Data;
using TaskManager.Modules.Tasks.Models;

namespace TaskManager.Modules.Tasks.Repository;

public class TaskRepository : ITaskRepository
{
    private readonly TasksDbContext _context;
    private readonly ILogger<TaskRepository> _logger;

    public TaskRepository(
        TasksDbContext context,
        ILogger<TaskRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<TaskItem>> GetAllAsync()
    {
        try
        {
            _logger.LogInformation("Fetching all tasks from database");

            return await _context.Tasks
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while fetching all tasks");
            throw;
        }
    }

    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("Fetching task with id {TaskId}", id);
            return await _context.Tasks.FirstOrDefaultAsync(x => x.Id == id);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while fetching task with id {TaskId}", id);
            throw;
        }
    }

    public async Task AddAsync(TaskItem task)
    {
        try
        {
            _logger.LogInformation("Adding new task with title {Title}", task.Title);

            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while adding task");
            throw;
        }
    }

    public async Task UpdateAsync(TaskItem task)
    {
        try
        {
            _logger.LogInformation("Updating task with id {TaskId}", task.Id);

            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while updating task with id {TaskId}", task.Id);
            throw;
        }
    }

    public async Task DeleteAsync(TaskItem task)
    {
        try
        {
            _logger.LogInformation("Deleting task with id {TaskId}", task.Id);

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting task with id {TaskId}", task.Id);
            throw;
        }
    }
}
