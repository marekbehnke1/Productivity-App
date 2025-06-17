using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using LearnAvalonia.Models;

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
    public static readonly StyledProperty<DateTime> DueDateProperty = AvaloniaProperty.Register<ListItem, DateTime>(nameof(DueDate));

    public DateTime DueDate
    {
        get => GetValue(DueDateProperty);
        set => SetValue(DueDateProperty, value);
    }
}

