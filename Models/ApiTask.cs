using System.ComponentModel.DataAnnotations;
using System;

namespace LearnAvaloniaApi.Models
{
    public class ApiTask
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int Priority { get; set; } = 0;

        [Required]
        public bool IsCollapsed { get; set; } = false;
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }
        public int? ProjectId { get; set; }

        public ApiTask() { }
        public ApiTask(string title, string description, int priority, bool isCollapsed, DateTime? dueDate, DateTime createdAt, DateTime updatedAt, int userId, int? projectId)
        {
            Title = title;
            Description = description;
            Priority = priority;
            IsCollapsed = isCollapsed;
            DueDate = dueDate;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            UserId = userId;
            ProjectId = projectId;
        }
    }
}
