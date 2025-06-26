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
        private int _id;

        private string _title = string.Empty;

        private string _description = string.Empty;

        private Priority _taskPriority;

        private DateTime? _dueDate;

        private bool _isCollapsed = false;

        [ObservableProperty]
        private int? _projectId;


        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
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

        public TaskItem(string title, string description, Priority taskPriority, DateTime? dueDate, int? projectId)
        {
            _title = title;
            _description = description;
            _taskPriority = taskPriority;
            _dueDate = dueDate;
            _isCollapsed = false;
            _projectId = projectId;
            //id does not need to be set here becase EFC sets it when a item is added to db
        }

    }
}
