using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnAvalonia.Models;

namespace LearnAvalonia.Models
{
    public class TaskItem
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Priority TaskPriority { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsCollapsed { get; set; } = false;

        public TaskItem() { }

        public TaskItem(string title, string description, Priority taskPriority, DateTime? dueDate)
        {
            Title = title;
            Description = description;
            TaskPriority = taskPriority;
            DueDate = dueDate;
            IsCollapsed = false;
        }
    }
}
