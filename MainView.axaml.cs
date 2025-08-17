using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Metadata;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using LearnAvalonia.Components;
using LearnAvalonia.Models;
using LearnAvalonia.Resources;
using LearnAvalonia.ViewModels;

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

    private void OnPasswordKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            if (DataContext is NavigationViewModel navVM &&
                navVM.CurrentViewModel is LoginViewModel loginVM)
            {
                if (loginVM.LoginCommand.CanExecute(null))
                {
                    loginVM.LoginCommand.Execute(null);
                }
            }
        }
    }
}