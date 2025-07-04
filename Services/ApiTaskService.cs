using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using LearnAvalonia.Models;
using LearnAvaloniaApi.Models;

namespace LearnAvalonia.Services
{
    internal class ApiTaskService : ITaskService
    {
        // This acts as our "dbcontext"
        private readonly HttpClient _httpClient;

        public ApiTaskService (HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Converts the ApiTask item type to the standard TaskItem type
        private TaskItem ConvertToTaskItem(ApiTask apiTask)
        {
            TaskItem taskItem = new TaskItem(
                apiTask.Title,
                apiTask.Description,
                (Priority)apiTask.Priority,
                apiTask.DueDate,
                apiTask.ProjectId
            );
            taskItem.Id = apiTask.Id;
            taskItem.IsCollapsed = apiTask.IsCollapsed;

            return taskItem;
        }

        //Converst TaskItem to ApiTask
        private ApiTask ConvertToApiTask(TaskItem task)
        {
            ApiTask apiTask = new ApiTask(
                task.Title,
                task.Description,
                (int)task.TaskPriority,
                task.IsCollapsed,                
                task.DueDate,
                DateTime.UtcNow,
                DateTime.UtcNow,
                1, //Userid is hardcoded for now - TODO: change when we add user auth
                task.ProjectId
                
            );
            apiTask.Id = task.Id;
            
            return apiTask;
        }

        private Project ConvertToProject(ApiProject apiProject)
        {
            Project project = new Project(
                apiProject.Name,
                apiProject.Description);

            project.Id = apiProject.Id;
            project.DateCreated = apiProject.DateCreated;

            return project;
        }
       

        public async Task<List<TaskItem>> GetAllTasksAsync()
        {
            // The type we are requesting from the api is ApiTask
            try
            {
                var apiTasks = await _httpClient.GetFromJsonAsync<List<ApiTask>>("api/tasks");
                // this returns each apitask, converted to a taskitem - with a default if its null
                return apiTasks?.Select(ConvertToTaskItem).ToList() ?? new List<TaskItem>();

            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"API is unreachable: {ex.Message}");
                return new List<TaskItem>();
            }
            
        }

        public async Task<List<TaskItem>> GetTasksByPriorityAsync(Priority priority)
        {
            var tasks = await GetAllTasksAsync();
            var filteredTasks = tasks.Where(x => 
                x.TaskPriority == priority).ToList(); 

            return filteredTasks;
        }

        public async Task<TaskItem> AddTaskAsync(TaskItem task)
        {
            var apiTask = ConvertToApiTask(task);
            var response = await _httpClient.PostAsJsonAsync("/api/tasks", apiTask);

            // throwns exception if not succesfull
            response.EnsureSuccessStatusCode();

            // This gets the newly created task back from the db
            var createdApiTask = await response.Content.ReadFromJsonAsync<ApiTask>();

            if (createdApiTask == null)
            {
                throw new HttpRequestException("Could not parse returned object");
            }

            return ConvertToTaskItem(createdApiTask);
        }

        public async Task<TaskItem> UpdateTaskAsync(TaskItem task)
        {
            var apiTask = ConvertToApiTask(task);

            var response = await _httpClient.PutAsJsonAsync($"/api/tasks/{task.Id}", apiTask);
            response.EnsureSuccessStatusCode();

            return task;
        }
        public async Task DeleteTaskAsync(int id)
        {

            //because the api returns no response - we just ust deleteasync
            var response = await _httpClient.DeleteAsync($"/api/tasks/{id}");

            // exceptions handled in viewmodel - this method throws an exception if not succesful
            response.EnsureSuccessStatusCode();

        }
        public async Task<TaskItem?> GetTaskByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ApiTask>($"/api/tasks/{id}");
                return response != null ? ConvertToTaskItem(response) : null ;
            }
            catch(HttpRequestException ex)
            {
                Console.WriteLine($"API could not be reached: {ex.Message}");
                return null;
            }

        }
        public async Task InitialiseDatabaseAsync()
        {
            // For the api service - the database does not have be initialised, as it is always running
            // So this is a no-method
            await Task.CompletedTask;
        }
        public async Task<List<Project>> GetProjectsAsync()
        {
            //throw new NotImplementedException("TODO: Implement API Call");
            try
            {
                var apiProjects = await _httpClient.GetFromJsonAsync<List<ApiProject>>("api/projects");
                return apiProjects?.Select(ConvertToProject).ToList() ?? new List<Project>();

            }
            catch(HttpRequestException ex)
            {
                Console.WriteLine($"API Could not be reached{ex.Message}");
                return new List<Project>();
            }
        }
        public async Task<Project> AddProjectAsync(Project project)
        {
            throw new NotImplementedException("TODO: Implement API Call");
        }
        public async Task<Project> UpdateProjectAsync(Project project)
        {
            throw new NotImplementedException("TODO: Implement API Call");
        }
        public async Task DeleteProjectAsync(int projectId)
        {
            throw new NotImplementedException("TODO: Implement API Call");
        }
        public async Task<List<TaskItem>> GetTasksByProjectAsync(int? projectId)
        {
            throw new NotImplementedException("TODO: Implement API Call");
        }

    }
}
