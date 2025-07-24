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
using LearnAvalonia.Models.Dtos;
using System.Net.Http;
using System.Diagnostics;

namespace LearnAvalonia.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly ITaskService _taskService;
        private readonly IAuthenticationService _authService;

        // This will be used to demonstrate when the db is loading
        [ObservableProperty]
        private bool _isLoading = false;

        // Will hold any error messages we need to display
        [ObservableProperty]
        private string _statusMessage = string.Empty;

        [ObservableProperty]
        private bool _hasMessage = false;

        [ObservableProperty]
        private bool _isSuccess;

        [ObservableProperty]
        private int _currentPanelIndex = 0;

        [ObservableProperty]
        private Project? _selectedProject;

        [ObservableProperty]
        private Priority? _selectedPriorityFilter;

        [ObservableProperty]
        private bool _showOnlyUncompleted;

        // This is the collection that the UI binds to
        // It is the single true data store for all task items
        public ObservableCollection<TaskItem> Tasks { get; set; }

        // Collection of all projects
        public ObservableCollection<Project> Projects { get; set; }

        // Current projects tasks
        public ObservableCollection<TaskItem> CurrentProjectTasks => new(Tasks.Where(t => t.ProjectId == SelectedProject?.Id));
        public ObservableCollection<TaskItem> CurrentFilteredTasks => new(
            CurrentProjectTasks.Where(t => 

                // Apply uncompleted filter if its active
                (!ShowOnlyUncompleted || t.TaskPriority != Priority.Complete) &&
                (SelectedPriorityFilter == null || t.TaskPriority == SelectedPriorityFilter.Value)
            )
        );

        public MainViewModel(ITaskService taskService, IAuthenticationService authService)
        {
            // Throw a new exception if we cannot load the task service
            _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));

            Tasks = new ObservableCollection<TaskItem>();
            Projects = new ObservableCollection<Project>();

            // Event is called whenever the tasks list is updated in any way
            Tasks.CollectionChanged += OnTasksCollectionChanged;
            Projects.CollectionChanged += OnProjectsCollectionChanged;

            _authService.AuthStateChanged += OnAuthStateChanged;

            // This begins loading the tasks & projects
            // Constructors cannot be async - so we have to call a one use async method.
            // _ = InitialiseAsync();

            // Temporary auth integration test
            _ = TestAuthentication(authService);

        }

        // Auth state changed event handler
        // This waits till after we have authorisation before loading the UI
        private async void OnAuthStateChanged(object? sender, AuthStateChangedEventArgs e)
        {
            Debug.WriteLine($"AuthStateChanged: IsAuth={e.IsAuthenticated}, Reason={e.ChangeReason}");

            if (e.IsAuthenticated && e.ChangeReason == AuthChangeReason.Login)
            {
                Debug.WriteLine($"User authenticated - loading UI data");

                // Now load the UI data after we are logged in
                await InitialiseAsync();
            }
            else if (!e.IsAuthenticated)
            {
                Debug.WriteLine("User logged out - clearing data");
                Tasks.Clear();
                Projects.Clear();
            }
        }

        // Test method for login
        private async Task TestAuthentication(IAuthenticationService authService)
        {
            try
            {
                var response = await authService.LoginAsync(new ApiLoginRequest()
                {
                    Email = "azure.test@example.com",
                    Password = "TestPassword123"
                });

            
                System.Diagnostics.Debug.WriteLine("-------- Auth Login test --------");
                System.Diagnostics.Debug.WriteLine($"Login Result: {response.Success}");
                System.Diagnostics.Debug.WriteLine($"Response Message:{response.Message}");
                System.Diagnostics.Debug.WriteLine($"Is Authenticated?:{authService.IsAuthenticated}");
                System.Diagnostics.Debug.WriteLine($"Current User:{authService.CurrentUser?.FirstName}");
                System.Diagnostics.Debug.WriteLine("-------- End Auth Login test --------");

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Test failed {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace {ex.StackTrace}");
            }
            System.Diagnostics.Debug.WriteLine("-------- Test Complete --------");
        }

        private async Task DisplayMessage(string message, bool success)
        {
            StatusMessage = message;
            HasMessage = true;
            IsSuccess = success;

            await Task.Delay(3000);

            StatusMessage = string.Empty;
            HasMessage = false;
        }

        // This initialises the async loading of data from the database
        private async Task InitialiseAsync()
        {
            try
            {
                IsLoading = true;

                await _taskService.InitialiseDatabaseAsync();

                await LoadTasksAsync();
                await LoadProjectsAsync();

            }
            catch (Exception ex)
            {
                await DisplayMessage($"Failed to initialise database: {ex.Message}", false);
                // If load fails, load sample data - otherwise the app will do nothing
                // LoadSampleData();

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

                //// Create some sample data if no tasks exist
                //if (Tasks.Count == 0)
                //{
                //    await CreateSampleDataAsync();
                //}
            }
            catch (Exception ex)
            {
                await DisplayMessage($"Failed to load tasks from database{ex.Message}", false);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoadProjectsAsync()
        {
            try
            {
                IsLoading = true;

                var projectsFromDb = await _taskService.GetProjectsAsync();
                Projects.Clear();

                foreach(var project in projectsFromDb)
                {
                    Projects.Add(project);
                }
            }
            catch (Exception ex)
            {
                await DisplayMessage($"Failed to load projects: {ex.Message}", false);
            }
            finally
            {
                IsLoading=false;
            }
        }

        // These samples are created if the database has no entries
        private async Task CreateSampleDataAsync()
        {
            var sampleTasks = new List<TaskItem>
            {
                new TaskItem("Complete Project Proposal", "Write and submit the Q4 project proposal document", Priority.High, DateTime.Now.AddDays(3), null),
                new TaskItem("Team Meeting", "Weekly standup with development team", Priority.Critical, DateTime.Now.AddDays(1), null),
                new TaskItem("Code Review", "Review pull requests from team members", Priority.Medium, DateTime.Now.AddDays(2), null),
                new TaskItem("Update Documentation", "Update API documentation for new features", Priority.Low, DateTime.Now.AddDays(1), null),
                new TaskItem("Research New Framework", "Investigate new UI framework options", Priority.Low, DateTime.Now.AddDays(2), null),
                new TaskItem("Create a thing", "Do Some other stuff", Priority.Complete, DateTime.Now.AddDays(7), null)
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
                    await DisplayMessage($"Failed to create sample tasks: {ex.Message}", false);
                }
            }
        }

        // This only gets called if nothing else works
        private void LoadSampleData()
        {
            var sampleTasks = new List<TaskItem>
            {
                new TaskItem("Complete Project Proposal", "Write and submit the Q4 project proposal document", Priority.High, DateTime.Now.AddDays(3), null),
                new TaskItem("Team Meeting", "Weekly standup with development team", Priority.Critical, DateTime.Now.AddDays(1), null),
                new TaskItem("Code Review", "Review pull requests from team members", Priority.Medium, DateTime.Now.AddDays(2), null),
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
                var newTask = new TaskItem("New Task", "", Priority.Low, DateTime.Today, null);

                newTask.ProjectId = SelectedProject?.Id;

                // Add task to db first
                var savedTask = await _taskService.AddTaskAsync(newTask);

                // Add task to UI list
                Tasks.Add(savedTask);
                await DisplayMessage("Task added succesfully", true);
            }
            catch (Exception ex)
            {
                await DisplayMessage($"{ex.Message}", false);
                Debug.WriteLine($"The exception msg is: {ex.Message}");
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
                await DisplayMessage("Task deleted succesfully", true);
            }
            catch (Exception ex)
            {
                await DisplayMessage($"Failed to delete task from database: {ex.Message}", false);

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
                await DisplayMessage($"Failed to update task: {ex.Message}", false);
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

            // Subscribe to new items
            if (e.NewItems != null)
            {
                foreach (TaskItem item in e.NewItems)
                {
                    item.PropertyChanged += OnTaskItemPropertyChanged;
                }
            }

            RefreshFilteredCollections();
        }

        // Triggers events whenever items in the collection are changed
        private void OnProjectsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            // Unsubscrive from old items
            if (e.OldItems != null)
            {
                foreach (Project item in e.OldItems)
                {
                    item.PropertyChanged -= OnProjectPropertyChanged;
                }
            }

            // Subscribe to new items
            if (e.NewItems != null)
            {
                foreach (Project item in e.NewItems)
                {
                    item.PropertyChanged += OnProjectPropertyChanged;
                }
            }
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

        private async void OnProjectPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is Project project)
            {
                await SaveProjectAsync(project);
            }
        }

        private async Task SaveProjectAsync(Project project)
        {
            try
            {
                await _taskService.UpdateProjectAsync(project);
            }
            catch (Exception ex)
            {
                await DisplayMessage($"Failed to update Project: {ex.Message}", false);
            }
        }
        private void RefreshFilteredCollections()
        {
            OnPropertyChanged(nameof(CurrentProjectTasks));
            OnPropertyChanged(nameof(CurrentFilteredTasks));
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

        public async Task AddNewProjectAsync(string title, string description)
        {
            try
            {
                var newProject = new Project(title, description); 

                // Adds the new project to database.
                var savedProject = await _taskService.AddProjectAsync(newProject);

                Projects.Add(savedProject); 

                // Sets the new project as current project
                SwitchToProject(savedProject);

                await DisplayMessage("Project created!", true);

            }
            catch (Exception ex)
            {
                await DisplayMessage($"Failed to add project: {ex.Message}", false);
            }
        }
        public async Task DeleteProjectAsync(Project project)
        {
            try
            {
                // create a list of all tasks to remove
                var tasksToremove = Tasks.Where(t => t.ProjectId == project.Id).ToList();
                foreach (var task in tasksToremove)
                {
                    Tasks.Remove(task);
                    await _taskService.DeleteTaskAsync(task.Id);
                }

                // Delete from database
                await _taskService.DeleteProjectAsync(project.Id);
                // Removes project from UI
                Projects.Remove(project);
                // Switch view to all
                SwitchToAll();


                await DisplayMessage("Project Deleted", true);
            }
            catch (Exception ex)
            {
                await DisplayMessage($"Unable to delete Project: {ex.Message}", false );
            }
            
        }
        public void SwitchToProject(Project project)
        {
            // Set current project to this project
            SelectedProject = project;
            SelectedPriorityFilter = null;
            RefreshFilteredCollections() ;
        }

        [RelayCommand]
        private void SwitchProject(Project project)
        {
            SwitchToProject(project);
        }

        //For the all projects button
        public void SwitchToAll()
        {
            SelectedProject = null;
            SelectedPriorityFilter = null;
            RefreshFilteredCollections();
        }

        [RelayCommand]
        private void SetCriticalFilter()
        {
            ShowOnlyUncompleted = false;
            SelectedPriorityFilter = Priority.Critical;
            RefreshFilteredCollections();
        }
        [RelayCommand]
        private void SetHighFilter()
        {
            ShowOnlyUncompleted = false;
            SelectedPriorityFilter = Priority.High;
            RefreshFilteredCollections();
        }
        [RelayCommand]
        private void SetMediumFilter()
        {
            ShowOnlyUncompleted = false;
            SelectedPriorityFilter = Priority.Medium;
            RefreshFilteredCollections();
        }
        [RelayCommand]
        private void SetLowFilter()
        {
            ShowOnlyUncompleted = false;
            SelectedPriorityFilter = Priority.Low;
            RefreshFilteredCollections();
        }
        [RelayCommand]
        private void SetNoFilter()
        {
            ShowOnlyUncompleted = false;
            SelectedPriorityFilter = null;
            RefreshFilteredCollections();
        }
        [RelayCommand]
        private void SetCompletedFilter()
        {
            ShowOnlyUncompleted = false;
            SelectedPriorityFilter = Priority.Complete;
            RefreshFilteredCollections();
        }

        [RelayCommand]
        private void SetUncompletedFilter()

        {
            ShowOnlyUncompleted = true;
            SelectedPriorityFilter = null;
            RefreshFilteredCollections();
        }
    }
    


}
