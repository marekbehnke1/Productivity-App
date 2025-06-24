using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnAvalonia.Models;

namespace LearnAvalonia.Services
{
    public interface ITaskService
    {
        //Returns list of all TaskItem objects
        Task<List<TaskItem>> GetAllTasksAsync();

        // Returns list of tasks filtered by priority
        Task<List<TaskItem>> GetTasksByPriorityAsync(Priority priority);

        //Adds a task to the DB
        Task<List<TaskItem>> AddTaskAsync(TaskItem task);

        //Updates a task in the db
        Task<List<TaskItem>> UpdateTaskAsync(TaskItem task);

        //Deletes a task specified by ID
        Task DeleteTaskAsync(int taskId);

        //Gets a specific task by ID
        Task<TaskItem?> GetTaskByIdAsync(int id);

        //Initialises the db, creates tables etc.
        // This will be called when the app starts
        Task InitialiseDatabaseAsync();
    }
}
