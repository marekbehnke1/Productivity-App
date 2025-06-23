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

namespace LearnAvalonia.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _test = "this is a testing string for databinding";

        // This is the collection that the UI binds to
        public ObservableCollection<TaskItem> Tasks { get; set; }
        public ObservableCollection<TaskItem> CriticalPrioTasks { get; set; }
        public ObservableCollection<TaskItem> HighPrioTasks { get; set; }
        public ObservableCollection<TaskItem> MediumPrioTasks { get; set; }
        public ObservableCollection<TaskItem> LowPrioTasks { get; set; }
        public ObservableCollection<TaskItem> CompletedTasks { get; set; }
        public MainViewModel()
        {
            Tasks = new ObservableCollection<TaskItem>();
            CriticalPrioTasks = new ObservableCollection<TaskItem>();
            HighPrioTasks = new ObservableCollection<TaskItem>();
            MediumPrioTasks = new ObservableCollection<TaskItem>();
            LowPrioTasks = new ObservableCollection<TaskItem>();
            CompletedTasks = new ObservableCollection<TaskItem>();

            // Function to populate the collection with data
            LoadTasks();
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

                if (task.TaskPriority == Priority.Critical)
                {
                    CriticalPrioTasks.Add(task);
                }
                else if (task.TaskPriority == Priority.High)
                {
                    HighPrioTasks.Add(task);
                }
                else if (task.TaskPriority == Priority.Medium)
                {
                    MediumPrioTasks.Add(task);
                }
                else if (task.TaskPriority == Priority.Low)
                {
                    LowPrioTasks.Add(task);
                }
                else if (task.TaskPriority == Priority.Complete)
                {
                    CompletedTasks.Add(task);
                }
            }
        }

        public void AddNewTask()
        {
            var newTask = new TaskItem("New Task","What do you need to do?", Priority.Low, DateTime.Today);
            Tasks.Add(newTask);
            // TODO: Code to add task to the filtered list


            
        }

        public void DeleteTask(TaskItem task)
        {
            Tasks.Remove(task);
            // Code to delete task from the filtered list
        }

    }
    


}
