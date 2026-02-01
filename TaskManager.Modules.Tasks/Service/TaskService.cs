using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TaskManager.Modules.Tasks.DTOs;
using TaskManager.Modules.Tasks.Models;
using TaskManager.Modules.Tasks.Repository;

namespace TaskManager.Modules.Tasks.Service;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _repository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TaskService(
        ITaskRepository repository,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    //READ USER ID FROM JWT (INT)
    private int CurrentUserId =>
        int.Parse(
            _httpContextAccessor.HttpContext!
                .User
                .FindFirst("id")!.Value
        );

    public async Task<List<TaskResponse>> GetAllAsync()
    {
        var tasks = await _repository.GetAllAsync();
        return _mapper.Map<List<TaskResponse>>(tasks);
    }

    public async Task<TaskResponse?> GetByIdAsync(int id)
    {
        var task = await _repository.GetByIdAsync(id);
        return task == null ? null : _mapper.Map<TaskResponse>(task);
    }

    public async Task<TaskResponse> CreateAsync(CreateTaskRequest request)
    {
        var task = _mapper.Map<TaskItem>(request);

        // ✅ required: set owner
        var userId = CurrentUserId;
        Console.WriteLine("USER ID FROM TOKEN: " + userId);
        task.UserId = userId;
        await _repository.AddAsync(task);
        return _mapper.Map<TaskResponse>(task);
    }

    public async Task<bool> UpdateAsync(int id, UpdateTaskRequest request)
    {
        var task = await _repository.GetByIdAsync(id);
        if (task == null) return false;

        _mapper.Map(request, task);
        await _repository.UpdateAsync(task);

        return true;
    }

    public async Task<bool> ToggleAsync(int id)
    {
        var task = await _repository.GetByIdAsync(id);
        if (task == null) return false;

        task.IsCompleted = !task.IsCompleted;
        await _repository.UpdateAsync(task);

        return true;
    }


    public async Task<bool> DeleteAsync(int id)
    {
        var task = await _repository.GetByIdAsync(id);
        if (task == null) return false;

        await _repository.DeleteAsync(task);
        return true;
    }
}
