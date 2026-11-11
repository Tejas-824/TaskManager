using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaskManager.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    AssignedTo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "AssignedTo", "CreatedDate", "Description", "IsDeleted", "ModifiedDate", "Priority", "Status", "Title" },
                values: new object[,]
                {
                    { 1, "Rahul", new DateTime(2026, 11, 2, 9, 0, 0, 0, DateTimeKind.Utc), "Create repo and add basic folder structure", false, new DateTime(2026, 11, 2, 9, 0, 0, 0, DateTimeKind.Utc), 3, 3, "Set up project repository" },
                    { 2, "Priya", new DateTime(2026, 11, 3, 9, 0, 0, 0, DateTimeKind.Utc), "Finalize tables and relations", false, new DateTime(2026, 11, 3, 9, 0, 0, 0, DateTimeKind.Utc), 3, 3, "Design database schema" },
                    { 3, "Amit", new DateTime(2026, 11, 4, 9, 0, 0, 0, DateTimeKind.Utc), "Login form with validation", false, new DateTime(2026, 11, 4, 9, 0, 0, 0, DateTimeKind.Utc), 4, 2, "Build login page" },
                    { 4, "Sneha", new DateTime(2026, 11, 5, 9, 0, 0, 0, DateTimeKind.Utc), "Dashboard takes too long to load", false, new DateTime(2026, 11, 5, 9, 0, 0, 0, DateTimeKind.Utc), 3, 2, "Fix dashboard loading bug" },
                    { 5, "Karan", new DateTime(2026, 11, 6, 9, 0, 0, 0, DateTimeKind.Utc), "Document all endpoints", false, new DateTime(2026, 11, 6, 9, 0, 0, 0, DateTimeKind.Utc), 2, 1, "Write API documentation" },
                    { 6, "Priya", new DateTime(2026, 11, 7, 9, 0, 0, 0, DateTimeKind.Utc), "Send email when a task is assigned", false, new DateTime(2026, 11, 7, 9, 0, 0, 0, DateTimeKind.Utc), 1, 1, "Add email notifications" },
                    { 7, "Rahul", new DateTime(2026, 11, 8, 9, 0, 0, 0, DateTimeKind.Utc), "Review pending PRs from the team", false, new DateTime(2026, 11, 8, 9, 0, 0, 0, DateTimeKind.Utc), 2, 1, "Review pull requests" },
                    { 8, "Amit", new DateTime(2026, 11, 9, 9, 0, 0, 0, DateTimeKind.Utc), "Payment page times out for some users", false, new DateTime(2026, 11, 9, 9, 0, 0, 0, DateTimeKind.Utc), 4, 1, "Fix payment timeout issue" },
                    { 9, "Sneha", new DateTime(2026, 11, 10, 9, 0, 0, 0, DateTimeKind.Utc), "Add screenshots for the new features", false, new DateTime(2026, 11, 10, 9, 0, 0, 0, DateTimeKind.Utc), 1, 3, "Update user guide" },
                    { 10, "Karan", new DateTime(2026, 11, 11, 9, 0, 0, 0, DateTimeKind.Utc), "Prepare backlog for the next sprint", false, new DateTime(2026, 11, 11, 9, 0, 0, 0, DateTimeKind.Utc), 2, 2, "Plan next sprint" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tasks");
        }
    }
}
