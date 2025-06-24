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
        private string _title = string.Empty;

        private string _description = string.Empty;

        private Priority _taskPriority;

        private DateTime? _dueDate;

        private bool _isCollapsed = false;


        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public Priority TaskPriority
        {
            get => _taskPriority;
            set => SetProperty(ref _taskPriority, value);
        }

        public DateTime? DueDate
        {
            get => _dueDate;
            set => SetProperty(ref _dueDate, value);
        }

        public bool IsCollapsed
        {
            get => _isCollapsed;
            set => SetProperty(ref _isCollapsed, value);
        }
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
