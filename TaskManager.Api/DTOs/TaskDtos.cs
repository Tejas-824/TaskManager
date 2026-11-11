using System.ComponentModel.DataAnnotations;
using TaskManager.Api.Models;

namespace TaskManager.Api.DTOs;

public class CreateTaskDto
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    [EnumDataType(typeof(TaskItemStatus), ErrorMessage = "Invalid status value")]
    public TaskItemStatus Status { get; set; } = TaskItemStatus.ToDo;

    [EnumDataType(typeof(TaskPriority), ErrorMessage = "Invalid priority value")]
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;

    [StringLength(100)]
    public string? AssignedTo { get; set; }
}

public class UpdateTaskDto : CreateTaskDto
{
}

public class TaskSummaryDto
{
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public int Total { get; set; }
}