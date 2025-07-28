using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using LearnAvalonia.Models;
using LearnAvalonia.Services;


namespace LearnAvalonia.ViewModels
{
    /// <summary>
    /// Design-time ViewModel that provides sample data for the XAML designer
    /// This allows Visual Studio's designer to show realistic preview data
    /// without requiring database connections or dependency injection
    /// </summary>
    public partial class DesignViewModel : ViewModelBase
    {
        public static DesignViewModel Instance { get; } = new DesignViewModel();
        public bool IsLoading { get; set; } = false;
        public string ErrorMessage { get; set; } = string.Empty;



        [ObservableProperty]
        private Project? _selectedProject;

        [ObservableProperty]
        private Priority? _selectedPriorityFilter;

        public ObservableCollection<TaskItem> Tasks { get; set; }
        public ObservableCollection<Project> Projects { get; set; }
        public ObservableCollection<TaskItem> CurrentProjectTasks => new(Tasks.Where(t => t.ProjectId == SelectedProject?.Id));
        public ObservableCollection<TaskItem> CurrentFilteredTasks => new(CurrentProjectTasks.Where(t => SelectedPriorityFilter == null || t.TaskPriority == SelectedPriorityFilter.Value));

        [ObservableProperty]
        private ViewModelBase? _currentViewModel;

        [ObservableProperty]
        private double _windowHeight = 300;

        [ObservableProperty]
        private LoginViewModel? _loginViewModel;

        /// <summary>
        /// Parameterless constructor that the designer can use
        /// Creates realistic sample data for design-time preview
        /// </summary>
        public DesignViewModel()
        {
            // Create sample tasks for design-time preview
            var sampleTasks = new[]
            {
                new TaskItem("Design Task 1", "This is a sample high priority task for design preview", Priority.High, DateTime.Now.AddDays(2), null),
                new TaskItem("Design Task 2", "This is a sample medium priority task", Priority.Medium, DateTime.Now.AddDays(5), null),
                new TaskItem("Design Task 3", "This is a sample low priority task with a longer description to show text wrapping", Priority.Low, DateTime.Now.AddDays(7), null),
                new TaskItem("Critical Issue", "Urgent task that needs immediate attention", Priority.Critical, DateTime.Now.AddDays(1), null),
                new TaskItem("Completed Work", "This task has been finished", Priority.Complete, DateTime.Now.AddDays(-2), null)
            };

            var sampleProjects = new[]
            {
                new Project("New Website", "Create a new Website for the business"),
                new Project("Productivity App", "A small productivity app using avalonia"),
                new Project("Stock Trading Site", "Online stocks trading platform, using python and flask")
            };

            // Initialize all collections
            Tasks = new ObservableCollection<TaskItem>(sampleTasks);
            Projects = new ObservableCollection<Project>(sampleProjects);

            
            CurrentViewModel = this;
            
        }

        // Empty methods for design-time (won't be called)
        public void AddNewTask() { }
        public void  AddNewProjectAsync(string title, string description) { }
        public void DeleteProjectAsync() { }
        public void SwitchToProject() { }
        public void DeleteTask(TaskItem task) { }
    }
}


