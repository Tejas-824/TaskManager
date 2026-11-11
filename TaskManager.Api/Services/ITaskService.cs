using TaskManager.Api.DTOs;
using TaskManager.Api.Models;

namespace TaskManager.Api.Services;

public interface ITaskService
{
    Task<List<TaskItem>> GetAllAsync(TaskItemStatus? status, TaskPriority? priority);
    Task<TaskItem?> GetByIdAsync(int id);
    Task<TaskItem> CreateAsync(CreateTaskDto dto);
    Task<TaskItem?> UpdateAsync(int id, UpdateTaskDto dto);
    Task<bool> DeleteAsync(int id);
    Task<List<TaskSummaryDto>> GetSummaryAsync();
}