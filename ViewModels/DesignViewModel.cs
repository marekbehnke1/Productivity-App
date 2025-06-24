using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using LearnAvalonia.Models;


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
        [ObservableProperty]
        private string _test = "Design-time test string";
        public int CurrentPanelIndex { get; set; } = 0;
        public bool IsLoading { get; set; } = false;
        public string ErrorMessage { get; set; } = string.Empty;

        public ObservableCollection<TaskItem> Tasks { get; set; }
        public ObservableCollection<TaskItem> CriticalPrioTasks { get; set; }
        public ObservableCollection<TaskItem> HighPrioTasks { get; set; }
        public ObservableCollection<TaskItem> MediumPrioTasks { get; set; }
        public ObservableCollection<TaskItem> LowPrioTasks { get; set; }
        public ObservableCollection<TaskItem> CompletedTasks { get; set; }

        /// <summary>
        /// Parameterless constructor that the designer can use
        /// Creates realistic sample data for design-time preview
        /// </summary>
        public DesignViewModel()
        {
            // Create sample tasks for design-time preview
            var sampleTasks = new[]
            {
                new TaskItem("Design Task 1", "This is a sample high priority task for design preview", Priority.High, DateTime.Now.AddDays(2)),
                new TaskItem("Design Task 2", "This is a sample medium priority task", Priority.Medium, DateTime.Now.AddDays(5)),
                new TaskItem("Design Task 3", "This is a sample low priority task with a longer description to show text wrapping", Priority.Low, DateTime.Now.AddDays(7)),
                new TaskItem("Critical Issue", "Urgent task that needs immediate attention", Priority.Critical, DateTime.Now.AddDays(1)),
                new TaskItem("Completed Work", "This task has been finished", Priority.Complete, DateTime.Now.AddDays(-2))
            };

            // Initialize all collections
            Tasks = new ObservableCollection<TaskItem>(sampleTasks);

            // Filter tasks by priority for design preview
            CriticalPrioTasks = new ObservableCollection<TaskItem>(
                sampleTasks.Where(t => t.TaskPriority == Priority.Critical));

            HighPrioTasks = new ObservableCollection<TaskItem>(
                sampleTasks.Where(t => t.TaskPriority == Priority.High));

            MediumPrioTasks = new ObservableCollection<TaskItem>(
                sampleTasks.Where(t => t.TaskPriority == Priority.Medium));

            LowPrioTasks = new ObservableCollection<TaskItem>(
                sampleTasks.Where(t => t.TaskPriority == Priority.Low));

            CompletedTasks = new ObservableCollection<TaskItem>(
                sampleTasks.Where(t => t.TaskPriority == Priority.Complete));
        }

        // Empty methods for design-time (won't be called)
        public void AddNewTask() { }
        public void DeleteTask(TaskItem task) { }
    }
}


