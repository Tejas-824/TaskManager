using System.Data;
using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Data;
using TaskManager.Api.DTOs;
using TaskManager.Api.Models;

namespace TaskManager.Api.Services;

public class TaskService : ITaskService
{
    private readonly AppDbContext _context;

    public TaskService(AppDbContext context)
    {
        _context = context;
    }

    // LINQ example: optional filters + sorting (highest priority first, then newest)
    public async Task<List<TaskItem>> GetAllAsync(TaskItemStatus? status, TaskPriority? priority)
    {
        var query = _context.Tasks.AsNoTracking().AsQueryable();

        if (status.HasValue)
            query = query.Where(t => t.Status == status.Value);

        if (priority.HasValue)
            query = query.Where(t => t.Priority == priority.Value);

        return await query
            .OrderByDescending(t => t.Priority)
            .ThenByDescending(t => t.CreatedDate)
            .ToListAsync();
    }

    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        return await _context.Tasks.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<TaskItem> CreateAsync(CreateTaskDto dto)
    {
        var now = DateTime.UtcNow;

        var task = new TaskItem
        {
            Title = dto.Title.Trim(),
            Description = dto.Description,
            Status = dto.Status,
            Priority = dto.Priority,
            AssignedTo = dto.AssignedTo,
            CreatedDate = now,
            ModifiedDate = now,
            IsDeleted = false
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        return task;
    }

    public async Task<TaskItem?> UpdateAsync(int id, UpdateTaskDto dto)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
        if (task == null) return null;

        task.Title = dto.Title.Trim();
        task.Description = dto.Description;
        task.Status = dto.Status;
        task.Priority = dto.Priority;
        task.AssignedTo = dto.AssignedTo;
        task.ModifiedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return task;
    }

    // soft delete: only mark as deleted, row stays in the database
    public async Task<bool> DeleteAsync(int id)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
        if (task == null) return false;

        task.IsDeleted = true;
        task.ModifiedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    // Raw SQL (no EF/LINQ): count of tasks grouped by status and priority
    public async Task<List<TaskSummaryDto>> GetSummaryAsync()
    {
        var result = new List<TaskSummaryDto>();
        var connection = _context.Database.GetDbConnection();

        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync();

        try
        {
            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT
                    CASE Status WHEN 1 THEN 'ToDo' WHEN 2 THEN 'InProgress' WHEN 3 THEN 'Done' END AS StatusName,
                    CASE Priority WHEN 1 THEN 'Low' WHEN 2 THEN 'Medium' WHEN 3 THEN 'High' WHEN 4 THEN 'Critical' END AS PriorityName,
                    COUNT(*) AS Total
                FROM Tasks
                WHERE IsDeleted = 0
                GROUP BY Status, Priority
                ORDER BY Status, Priority";

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(new TaskSummaryDto
                {
                    Status = reader.GetString(0),
                    Priority = reader.GetString(1),
                    Total = reader.GetInt32(2)
                });
            }
        }
        finally
        {
            await connection.CloseAsync();
        }

        return result;
    }
}