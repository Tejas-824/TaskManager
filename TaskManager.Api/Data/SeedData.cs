using TaskManager.Api.Models;

namespace TaskManager.Api.Data;

public static class SeedData
{
    public static List<TaskItem> GetTasks()
    {
        var baseDate = new DateTime(2026, 11, 1, 9, 0, 0, DateTimeKind.Utc);

        TaskItem Make(int id, string title, string description,
            TaskItemStatus status, TaskPriority priority, string assignedTo)
        {
            return new TaskItem
            {
                Id = id,
                Title = title,
                Description = description,
                Status = status,
                Priority = priority,
                AssignedTo = assignedTo,
                CreatedDate = baseDate.AddDays(id),
                ModifiedDate = baseDate.AddDays(id),
                IsDeleted = false
            };
        }

        return new List<TaskItem>
        {
            Make(1, "Set up project repository", "Create repo and add basic folder structure", TaskItemStatus.Done, TaskPriority.High, "Rahul"),
            Make(2, "Design database schema", "Finalize tables and relations", TaskItemStatus.Done, TaskPriority.High, "Priya"),
            Make(3, "Build login page", "Login form with validation", TaskItemStatus.InProgress, TaskPriority.Critical, "Amit"),
            Make(4, "Fix dashboard loading bug", "Dashboard takes too long to load", TaskItemStatus.InProgress, TaskPriority.High, "Sneha"),
            Make(5, "Write API documentation", "Document all endpoints", TaskItemStatus.ToDo, TaskPriority.Medium, "Karan"),
            Make(6, "Add email notifications", "Send email when a task is assigned", TaskItemStatus.ToDo, TaskPriority.Low, "Priya"),
            Make(7, "Review pull requests", "Review pending PRs from the team", TaskItemStatus.ToDo, TaskPriority.Medium, "Rahul"),
            Make(8, "Fix payment timeout issue", "Payment page times out for some users", TaskItemStatus.ToDo, TaskPriority.Critical, "Amit"),
            Make(9, "Update user guide", "Add screenshots for the new features", TaskItemStatus.Done, TaskPriority.Low, "Sneha"),
            Make(10, "Plan next sprint", "Prepare backlog for the next sprint", TaskItemStatus.InProgress, TaskPriority.Medium, "Karan")
        };
    }
}