using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
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

        //Converts TaskItem to ApiTask
        private ApiTask ConvertToApiTask(TaskItem task)
        {
            ApiTask apiTask = new ApiTask(
                task.Title,
                task.Description,
                (int)task.TaskPriority,
                task.IsCollapsed,                
                task.DueDate,
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
                apiProject.Description
            );

            project.Id = apiProject.Id;
            project.DateCreated = apiProject.DateCreated;

            return project;
        }

        private ApiProject ConvertToApiProject(Project project)
        {
            ApiProject apiProject = new ApiProject(
                project.Name,
                project.Description,
                1 //This is hardcoded for now until we add auth - TODO: Change when auth is added
            );
            apiProject.Id = project.Id;
            apiProject.DateCreated = project.DateCreated;

            return apiProject;
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

            var json = JsonSerializer.Serialize(apiTask);
            System.Diagnostics.Debug.WriteLine($"Adding Task from ApiTaskService: {json}");
            System.Diagnostics.Debug.WriteLine($"Sending Json: {json}");


            var response = await _httpClient.PostAsJsonAsync("/api/tasks", apiTask);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"API Error: {response.StatusCode}");
                System.Diagnostics.Debug.WriteLine($"Error Details: {errorContent}");
            }

            // throws exception if not succesfull
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
            // Convert project
            var apiProject = ConvertToApiProject(project);
            // Send Project
            var response = await _httpClient.PostAsJsonAsync("/api/projects", apiProject);

            //Ensure successul send
            response.EnsureSuccessStatusCode();

            // store returned data
            var createdProject = await response.Content.ReadFromJsonAsync<ApiProject>();

            // check for null
            if (createdProject == null)
            {
                throw new HttpRequestException("Could not parse project");
            }

            // return as converted
            return ConvertToProject(createdProject);
        }
        public async Task<Project> UpdateProjectAsync(Project project)
        {
            // Convert to Api type
            var apiProject = ConvertToApiProject(project);
            // Send to api
            var response = await _httpClient.PutAsJsonAsync($"/api/projects/{project.Id}", apiProject);
            // check response
            response.EnsureSuccessStatusCode();

            // return if ok
            return ConvertToProject(apiProject);
        }
        public async Task DeleteProjectAsync(int projectId)
        {
            // send delete request
            var response = await _httpClient.DeleteAsync($"/api/projects/{projectId}");

            // check response
            response.EnsureSuccessStatusCode();
        }
        public async Task<List<TaskItem>> GetTasksByProjectAsync(int? projectId)
        {
            var tasks = await GetAllTasksAsync();
            
            // LINQ to filter the tasks list
            var filteredTasks = tasks.Where(task => 
                                task.ProjectId == projectId)
                                .ToList();

            return filteredTasks;
        }

    }
}
