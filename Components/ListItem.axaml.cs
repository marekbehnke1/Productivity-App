using System;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using LearnAvalonia.Models;
using LearnAvalonia.ViewModels;

namespace LearnAvalonia.Components;

public partial class ListItem : UserControl
{
    public ListItem()
    {
       InitializeComponent();
       PrioritySelector.ItemsSource = Enum.GetValues(typeof(Priority));
    }

    // Registering the title as a stylable property
    public static readonly StyledProperty<string> TitleProperty = AvaloniaProperty.Register<ListItem, string>(nameof(Title));

    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    // Registering the priority
    public static readonly StyledProperty<Priority> TaskPriorityProperty = AvaloniaProperty.Register<ListItem, Priority>(nameof(TaskPriority));

    public Priority TaskPriority
    {
        get => GetValue(TaskPriorityProperty);
        set => SetValue(TaskPriorityProperty, value);
    }

    // Registering the Description
    public static readonly StyledProperty<string> DescriptionProperty = AvaloniaProperty.Register<ListItem, string>(nameof(Description));

    public string Description
    {
        get => GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    // Registering the Due date
    public static readonly StyledProperty<DateTime?> DueDateProperty =
        AvaloniaProperty.Register<ListItem, DateTime?>(
            nameof(DueDate),
            defaultValue: null);
    public DateTime? DueDate
    {
        get => GetValue(DueDateProperty);
        set => SetValue(DueDateProperty, value);
    }

    // Registering Collapse Property
    public static readonly StyledProperty<bool> IsCollapsedProperty =
        AvaloniaProperty.Register<ListItem, bool>(nameof(IsCollapsed));
       
    public bool IsCollapsed
    {
        get => GetValue(IsCollapsedProperty);
        set => SetValue(IsCollapsedProperty, value);
    }

    //event listener for the collapse button
    private void Collapse(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if(IsCollapsed == true)
        {
            IsCollapsed = false;
        }
        else
        {
            IsCollapsed = true;
        }
    }

    //event listener for the delete button.
    // find the first ancestor with type of MainView
    // 
    private void DeleteTask(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var mainView = this.FindAncestorOfType<MainView>();
        if (mainView?.DataContext is NavigationViewModel viewModel && this.DataContext is TaskItem task)
        {
            if (viewModel.CurrentViewModel is MainViewModel mainViewModel)
            {
                mainViewModel?.DeleteTaskAsync(task);
            }
        }
    }

}

