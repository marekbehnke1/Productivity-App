using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnAvalonia.Models;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;

namespace LearnAvalonia.Models
{
    public partial class TaskItem : ObservableObject
    {
        [ObservableProperty]
        private string _title = string.Empty;

        [ObservableProperty]
        private string _description = string.Empty;

        [ObservableProperty]
        private Priority _taskPriority;

        [ObservableProperty]
        private DateTime? _dueDate;

        [ObservableProperty]
        private bool _isCollapsed = false;

        public TaskItem() { }

        public TaskItem(string title, string description, Priority taskPriority, DateTime? dueDate)
        {
            _title = title;
            _description = description;
            _taskPriority = taskPriority;
            _dueDate = dueDate;
            _isCollapsed = false;
        }

    }
}
