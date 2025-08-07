using System;
using System.Threading;
using System.Timers;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Metadata;
using Avalonia.Threading;
using LearnAvalonia.ViewModels;
using LearnAvalonia.Components;
using System.Threading.Tasks;
using LearnAvalonia.Models;
using System.Collections.Generic;
using System.Drawing;
using Avalonia.Media;
using LearnAvalonia.Resources;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;

namespace LearnAvalonia;

public partial class MainView : Window
{
    public MainView()
    {
        InitializeComponent();
    }
   
    // Registering Collapse Property
    public static readonly StyledProperty<bool> IsCollapsedProperty =
        AvaloniaProperty.Register<MainView, bool>(nameof(IsCollapsed));


    public bool IsCollapsed
    {
        get => GetValue(IsCollapsedProperty);
        set => SetValue(IsCollapsedProperty, value);

    }

}