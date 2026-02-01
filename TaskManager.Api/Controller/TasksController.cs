using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Modules.Tasks.DTOs;
using TaskManager.Modules.Tasks.Service;

namespace TaskManager.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/tasks")]
[Produces("application/json")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    // GET: api/v1/tasks
    [HttpGet]
    [ProducesResponseType(typeof(List<TaskResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<TaskResponse>>> GetAll()
    {
        return Ok(await _taskService.GetAllAsync());
    }

    // GET: api/v1/tasks/{id}
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskResponse>> GetById(int id)
    {
        var task = await _taskService.GetByIdAsync(id);

        if (task is null)
            return NotFound(new { message = $"Task not found with Id {id}" });

        return Ok(task);
    }

    // POST: api/v1/tasks
    [HttpPost]
    [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TaskResponse>> Create([FromBody] CreateTaskRequest request)
    {
        var task = await _taskService.CreateAsync(request);

        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
    }

    // PUT: api/v1/tasks/{id}
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskRequest request)
    {
        var updated = await _taskService.UpdateAsync(id, request);

        if (!updated)
            return NotFound(new { message = "Task not found for Update" });

        return NoContent();
    }

    // Toggle: api/v1/tasks/{id}
    [HttpPatch("{id:int}/toggle")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Toggle(int id)
    {
        var updated = await _taskService.ToggleAsync(id);

        if (!updated)
            return NotFound(new { message = "Task not found" });

        return NoContent();
    }


    // DELETE: api/v1/tasks/{id}
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _taskService.DeleteAsync(id);

        if (!deleted)
            return NotFound(new { message = "Task not found for Delete" });

        return NoContent();
    }
}
