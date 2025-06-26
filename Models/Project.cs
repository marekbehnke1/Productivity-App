using System;
using System.Collections.Generic;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;

namespace LearnAvalonia.Models
{
    public partial class Project : ObservableObject
    {
        [ObservableProperty]
        private int _id;

        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private string _description = string.Empty;

        [ObservableProperty]
        private DateTime _dateCreated;

        public Project() { }
        public Project(string name, string description)
        {
            _name = name;
            _description = description;
            _dateCreated = DateTime.Now;
        }
    }
}
