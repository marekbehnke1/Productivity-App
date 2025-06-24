using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LearnAvalonia.Data;
using LearnAvalonia.Models;

namespace LearnAvalonia.Services
{
    internal class TaskService : ITaskService
    {
        public async Task InitialiseDatabaseAsync()
        {
            using var context = new TaskDbContext();

            await context.Database.EnsureCreatedAsync();
        }

        public async Task<List<TaskItem>> GetAllTasksAsync()
        {
            using var context = new TaskDbContext();

            return await context.Tasks.ToListAsync();
        }

        public async Task <List<TaskItem>> GetTasksByPriorityAsync(Priority priority)
        {
            using var context = new TaskDbContext();

            return await context.Tasks
                .Where(t => t.TaskPriority == priority)
                .ToListAsync();
        }

        public async Task<TaskItem> AddTaskAsync(TaskItem task)
        {
            using var context = new TaskDbContext();
            context.Tasks.Add(task);

            await context.SaveChangesAsync();
            return task;
        }

        public async Task<TaskItem> UpdateTaskAsync(TaskItem task)
        {
            using var context = new TaskDbContext();
            context.Tasks.Update(task);
            await context.SaveChangesAsync();

            return task;
        }

        public async Task DeleteTaskAsync(int taskId)
        {
            using var context = new TaskDbContext();

            var task = await context.Tasks.FindAsync(taskId);

            if (task != null)
            {
                context.Tasks.Remove(task);
                await context.SaveChangesAsync();

            }
        }

        public async Task<TaskItem?> GetTaskByIdAsync(int taskId)
        {
            using var context = new TaskDbContext();

            return await context.Tasks.FindAsync(taskId);
        } 
    }
}
