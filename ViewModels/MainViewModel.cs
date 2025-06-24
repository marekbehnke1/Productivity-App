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
using LearnAvalonia.Services;

namespace LearnAvalonia.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly ITaskService _taskService;

        // This will be used to demonstrate when the db is loading
        [ObservableProperty]
        private bool _isLoading = false;

        // Will hold any error messages we need to display
        [ObservableProperty]
        private string _errorMessage = string.Empty;

        [ObservableProperty]
        private int _currentPanelIndex = 0;

        // This is the collection that the UI binds to
        // It is the single true data store for all task items
        public ObservableCollection<TaskItem> Tasks { get; set; }

        // These collections are filtered versions of the main tasks collections, where a certain condition is met
        public ObservableCollection<TaskItem> CriticalPrioTasks => new(Tasks.Where(t => t.TaskPriority == Priority.Critical));
        public ObservableCollection<TaskItem> HighPrioTasks => new(Tasks.Where(t => t.TaskPriority == Priority.High));
        public ObservableCollection<TaskItem> MediumPrioTasks => new(Tasks.Where(t => t.TaskPriority == Priority.Medium));
        public ObservableCollection<TaskItem> LowPrioTasks => new(Tasks.Where(t => t.TaskPriority == Priority.Low));
        public ObservableCollection<TaskItem> CompletedTasks => new(Tasks.Where(t => t.TaskPriority == Priority.Complete));
        public MainViewModel(ITaskService taskService)
        {
            // Throw a new exception if we cannot load the task service
            _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));

            Tasks = new ObservableCollection<TaskItem>();

            // Event is called whenever the tasks list is updated in any way
            Tasks.CollectionChanged += OnTasksCollectionChanged;

            // This begins loading the tasks
            // Constructors cannot be async - so we have to call a one use async method.
            _ = InitialiseAsync();

            // Function to populate the collection with data
            //LoadTasks();

            //foreach (var task in Tasks)
            //{
            //    task.PropertyChanged += OnTaskItemPropertyChanged;
            //}
        }

        // This initialises the async loading of data from the database
        private async Task InitialiseAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                await _taskService.InitialiseDatabaseAsync();

                await LoadTasksAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Failed to initialise database: {ex.Message}";
                // If load fails, load sample data - otherwise the app will do nothing
                LoadSampleData();
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoadTasksAsync()
        {
            try
            {
                IsLoading = true;

                // Load all the tasks from database
                var tasksFromDb = await _taskService.GetAllTasksAsync();
                // Clear existing tasks
                Tasks.Clear();

                // Load db tasks into app
                foreach (var task in tasksFromDb)
                {
                    Tasks.Add(task);
                }

                // Create some sample data if no tasks exist
                if (Tasks.Count == 0)
                {
                    await CreateSampleDataAsync();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Failed to load tasks from database{ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        // These samples are created if the database has no entries
        private async Task CreateSampleDataAsync()
        {
            var sampleTasks = new List<TaskItem>
            {
                new TaskItem("Complete Project Proposal", "Write and submit the Q4 project proposal document", Priority.High, DateTime.Now.AddDays(3)),
                new TaskItem("Team Meeting", "Weekly standup with development team", Priority.Critical, DateTime.Now.AddDays(1)),
                new TaskItem("Code Review", "Review pull requests from team members", Priority.Medium, DateTime.Now.AddDays(2)),
                new TaskItem("Update Documentation", "Update API documentation for new features", Priority.Low, DateTime.Now.AddDays(1)),
                new TaskItem("Research New Framework", "Investigate new UI framework options", Priority.Low, DateTime.Now.AddDays(2)),
                new TaskItem("Create a thing", "Do Some other stuff", Priority.Complete, DateTime.Now.AddDays(7))
            };

            // Add the sample tasks to the database
            foreach (var task in sampleTasks)
            {
                try
                {
                    var savedTask = await _taskService.AddTaskAsync(task);
                    Tasks.Add(savedTask);
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Failed to create sample tasks: {ex.Message}";
                }
            }
        }

        // This only gets called if nothing else works
        private void LoadSampleData()
        {
            var sampleTasks = new List<TaskItem>
            {
                new TaskItem("Complete Project Proposal", "Write and submit the Q4 project proposal document", Priority.High, DateTime.Now.AddDays(3)),
                new TaskItem("Team Meeting", "Weekly standup with development team", Priority.Critical, DateTime.Now.AddDays(1)),
                new TaskItem("Code Review", "Review pull requests from team members", Priority.Medium, DateTime.Now.AddDays(2)),
            };

            Tasks.Clear();

            foreach (var task in sampleTasks)
            {
                Tasks.Add(task);
            }
        }

        public async Task AddNewTaskAsync()
        {
            try
            {
                var newTask = new TaskItem("New Task", "What do you want to do?", Priority.Low, DateTime.Today);

                // Add task to db first
                var savedTask = await _taskService.AddTaskAsync(newTask);

                // Add task to UI list
                Tasks.Add(savedTask);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Could not add task to database: {ex.Message}"; 
            }
        }

        public async Task DeleteTaskAsync(TaskItem task)
        {
            try
            {
                // Remove task from DB
                await _taskService.DeleteTaskAsync(task.Id);

                // remove task from UI
                Tasks.Remove(task);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Failed to delete task from database: {ex.Message}";

                // If database delete failed, but task was removed from UI - reload the UI
                await LoadTasksAsync();
            }
        }

        public async Task SaveTaskAsync(TaskItem task)
        {
            try
            {
                await _taskService.UpdateTaskAsync(task);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Failed to update task: {ex.Message}";
            }
        }


        // This method handles when items are added/removed from lists
        private void OnTasksCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {

            // Unsubscribe from the old items
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
        private async void OnTaskItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            // Save changes to database when changes are made to items
            if (sender is TaskItem task)
            {
                await SaveTaskAsync(task);
            }

            //Only update the lists when you change the priority of tasks
            if (e.PropertyName == nameof(TaskItem.TaskPriority))
            {
                // Refresh the collections when priority is changed
                RefreshFilteredCollections();
            }
        }
        private void RefreshFilteredCollections()
        {
            OnPropertyChanged(nameof(CriticalPrioTasks));
            OnPropertyChanged(nameof(HighPrioTasks));
            OnPropertyChanged(nameof(MediumPrioTasks));
            OnPropertyChanged(nameof(LowPrioTasks));
            OnPropertyChanged(nameof(CompletedTasks));
        }

        // Legacy methods for backwards compatability
        public void AddNewTask()
        {
            _ = AddNewTaskAsync();
        }

        public void DeleteTask(TaskItem task)
        {
            _ = DeleteTaskAsync(task);
        }

    }
    


}
