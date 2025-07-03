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
            throw new NotImplementedException("TODO: Implement API Call");
        }
        public async Task<TaskItem> AddTaskAsync(TaskItem task)
        {
            throw new NotImplementedException("TODO: Implement API Call");
        }
        public async Task<TaskItem> UpdateTaskAsync(TaskItem task)
        {
            throw new NotImplementedException("TODO: Implement API Call");
        }
        public async Task DeleteTaskAsync(int id)
        {
            throw new NotImplementedException("TODO: Implement API Call");
        }
        public async Task<TaskItem?> GetTaskByIdAsync(int id)
        {
            throw new NotImplementedException("TODO: Implement API Call");
        }
        public async Task InitialiseDatabaseAsync()
        {
            throw new NotImplementedException("TODO: Implement API Call");
        }
        public async Task<List<Project>> GetProjectsAsync()
        {
            throw new NotImplementedException("TODO: Implement API Call");
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
