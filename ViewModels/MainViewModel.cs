using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LearnAvalonia.Components;
using LearnAvalonia.Models;
using System.ComponentModel;
using System.Collections.Specialized;

namespace LearnAvalonia.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _test = "this is a testing string for databinding";

        [ObservableProperty]
        private int _currentPanelIndex = 0;

        // This is the collection that the UI binds to
        public ObservableCollection<TaskItem> Tasks { get; set; }

        // These collections are filtered versions of the main tasks collections, where a certain condition is met
        public ObservableCollection<TaskItem> CriticalPrioTasks => new(Tasks.Where(t => t.TaskPriority == Priority.Critical));
        public ObservableCollection<TaskItem> HighPrioTasks => new(Tasks.Where(t => t.TaskPriority == Priority.High));
        public ObservableCollection<TaskItem> MediumPrioTasks => new(Tasks.Where(t => t.TaskPriority == Priority.Medium));
        public ObservableCollection<TaskItem> LowPrioTasks => new(Tasks.Where(t => t.TaskPriority == Priority.Low));
        public ObservableCollection<TaskItem> CompletedTasks => new(Tasks.Where(t => t.TaskPriority == Priority.Complete));
        public MainViewModel()
        {
            Tasks = new ObservableCollection<TaskItem>();

            // Event is called whenever the tasks list is updated in any way
            Tasks.CollectionChanged += (s, e) => RefreshFilteredCollections();

            // Function to populate the collection with data
            LoadTasks();
        }

        private void RefreshFilteredCollections()
        {
            System.Diagnostics.Debug.WriteLine("RefreshFilteredCollections called");

            OnPropertyChanged(nameof(CriticalPrioTasks));
            OnPropertyChanged(nameof(HighPrioTasks));
            OnPropertyChanged(nameof(MediumPrioTasks));
            OnPropertyChanged(nameof(LowPrioTasks));
            OnPropertyChanged(nameof(CompletedTasks));
        }

        // This method handles when items are added/removed from lists
        private void OnTasksCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            // Unsubscrive from the old items
            if (e.OldItems != null)
            {
                foreach (TaskItem item in e.OldItems)
                {
                    item.PropertyChanged -= OnTaskItemPropertyChanged;
                }
            }

            if (e.NewItems != null)
            {
                foreach (TaskItem item in e.NewItems)
                {
                    item.PropertyChanged += OnTaskItemPropertyChanged;
                }
            }

            RefreshFilteredCollections();
        }

        // This method handles when individual items properties change
        private void OnTaskItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Property changed: {e.PropertyName}");
            System.Diagnostics.Debug.WriteLine($"Sender: {sender?.GetType().Name}");
            // When Any TaskItem property changes, refresh the filtered lists
            RefreshFilteredCollections();
        }
        
        private void LoadTasks()
        {

            // Clears existing data
            Tasks.Clear();

            // this data will be replaced with database call
            var placeHolderTasks = new List<TaskItem>()
            {
                new TaskItem("Complete Project Proposal", "Write and submit the Q4 project proposal document", Priority.High, DateTime.Now.AddDays(3)),
                new TaskItem("Team Meeting", "Weekly standup with development team", Priority.Critical, DateTime.Now.AddDays(1)),
                new TaskItem("Code Review", "Review pull requests from team members", Priority.Medium, DateTime.Now.AddDays(2)),
                new TaskItem("Update Documentation", "Update API documentation for new features", Priority.Low, DateTime.Now.AddDays(1)),
                new TaskItem("Research New Framework", "Investigate new UI framework options", Priority.Low, DateTime.Now.AddDays(2))
            };

            foreach (var task in placeHolderTasks)
            {
                Tasks.Add(task);
            }
        }

        public void AddNewTask()
        {
            var newTask = new TaskItem("New Task","What do you need to do?", Priority.Low, DateTime.Today);
            Tasks.Add(newTask);
        }

        public void DeleteTask(TaskItem task)
        {
            // Remove task from Main list
            Tasks.Remove(task);
        }

    }
    


}
