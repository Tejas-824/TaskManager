using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.DTOs;
using TaskManager.Api.Models;
using TaskManager.Api.Services;

namespace TaskManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _service;

    public TasksController(ITaskService service)
    {
        _service = service;
    }

    // GET /api/tasks?status=ToDo&priority=High
    [HttpGet]
    public async Task<ActionResult<List<TaskItem>>> GetAll([FromQuery] TaskItemStatus? status, [FromQuery] TaskPriority? priority)
    {
        return Ok(await _service.GetAllAsync(status, priority));
    }

    // GET /api/tasks/summary
    [HttpGet("summary")]
    public async Task<ActionResult<List<TaskSummaryDto>>> GetSummary()
    {
        return Ok(await _service.GetSummaryAsync());
    }

    // GET /api/tasks/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskItem>> GetById(int id)
    {
        var task = await _service.GetByIdAsync(id);
        if (task == null) return TaskNotFound(id);
        return Ok(task);
    }

    // POST /api/tasks
    [HttpPost]
    public async Task<ActionResult<TaskItem>> Create(CreateTaskDto dto)
    {
        var task = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
    }

    // PUT /api/tasks/5
    [HttpPut("{id:int}")]
    public async Task<ActionResult<TaskItem>> Update(int id, UpdateTaskDto dto)
    {
        var task = await _service.UpdateAsync(id, dto);
        if (task == null) return TaskNotFound(id);
        return Ok(task);
    }

    // DELETE /api/tasks/5 (soft delete)
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted) return TaskNotFound(id);
        return NoContent();
    }

    private ActionResult TaskNotFound(int id)
    {
        return Problem(statusCode: 404, title: "Task not found", detail: $"No task found with id {id}");
    }
}