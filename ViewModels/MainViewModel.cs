using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
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

        public MainViewModel()
        {
            Tasks = new ObservableCollection<TaskItem>();

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
                new TaskItem("Complete Project Proposal", "Write and submit the Q4 project proposal document", Priority.High, DateTime.Now.AddDays(3), false),
                new TaskItem("Team Meeting", "Weekly standup with development team", Priority.Medium, DateTime.Now.AddDays(1), false),
                new TaskItem("Code Review", "Review pull requests from team members", Priority.Medium, DateTime.Now.AddDays(2), false),
                new TaskItem("Update Documentation", "Update API documentation for new features", Priority.Low, DateTime.Now.AddDays(1), false),
                new TaskItem("Research New Framework", "Investigate new UI framework options", Priority.Low, DateTime.Now.AddDays(2), false)
            };

            foreach (var task in placeHolderTasks)
            {
                Tasks.Add(task);
            }

        }

    }
    


}
