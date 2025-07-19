using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LearnAvalonia.Data;
using LearnAvalonia.Models;
using System.ComponentModel.DataAnnotations;

// This service only related to local DB storage - not through the API
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

        public async Task<List<TaskItem>> GetTasksByPriorityAsync(Priority priority)
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

        public async Task<List<Project>> GetProjectsAsync()
        {
            using var context = new TaskDbContext();

            return await context.Projects.ToListAsync();
        }

        public async Task<Project> AddProjectAsync(Project project)
        {
            using var context = new TaskDbContext();

            context.Projects.Add(project);

            await context.SaveChangesAsync();

            return project;

        }

        public async Task<Project> UpdateProjectAsync(Project project)
        {
            using var context = new TaskDbContext();

            context.Projects.Update(project);
            await context.SaveChangesAsync();

            return project;
        }

        public async Task DeleteProjectAsync(int projectId)
        {
            using var context = new TaskDbContext();

            var project = await context.Projects.FindAsync(projectId);

            if (project != null)
            {
                context.Remove(project);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<TaskItem>> GetTasksByProjectAsync(int? projectId)
        {
            using var context = new TaskDbContext();

            return await context.Tasks
                .Where(t => t.ProjectId == projectId)
                .ToListAsync();
        }
    }
}
