using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.VisualTree;
using LearnAvalonia.Components;
using LearnAvalonia.Models;
using LearnAvalonia.ViewModels;

namespace LearnAvalonia.Components;

public partial class ProjectHeader : UserControl
{
    public ProjectHeader()
    {
        InitializeComponent();
    }

    private async void DeleteProject(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var mainView = this.FindAncestorOfType<MainView>();
        if (mainView?.DataContext is MainViewModel viewModel && 
            this.DataContext is Project project)
        {
            var result = await ShowConfirmationDialog(mainView, project.Name);

            if (result)
            {
                await viewModel.DeleteProjectAsync(project);
            }
        }
    }

    private async Task<bool> ShowConfirmationDialog(Window parent, string projectName)
    {
        var dialog = new Window
        {
            Title = "Delete Project",
            Width = 400,
            Height = 200,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false,
            SystemDecorations = SystemDecorations.Full
        };

        var content = new StackPanel
        {
            Margin = new Thickness(20),
            Spacing = 20,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
        };

        content.Children.Add(new TextBlock
        {
            Text = $"Are you sure you want to delete the project '{projectName}'?",
            TextWrapping = TextWrapping.Wrap,
            TextAlignment = TextAlignment.Center,
            FontSize = 16
        });

        content.Children.Add(new TextBlock
        {
            Text = "This action cannot be undone.",
            TextAlignment = TextAlignment.Center,
            FontSize = 14,
            Foreground = Brushes.Orange
        });

        var buttonPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Spacing = 10,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
        };

        var yesButton = new Button
        {
            Content = "Yes, Delete",
            Background = Brushes.Red,
            Foreground = Brushes.White,
            Padding = new Thickness(15, 5)
        };

        var noButton = new Button
        {
            Content = "Cancel",
            Padding = new Thickness(15, 5)
        };

        bool result = false;

        yesButton.Click += (s, e) =>
        {
            result = true;
            dialog.Close();
        };

        noButton.Click += (s, e) =>
        {
            result = false;
            dialog.Close();
        };

        buttonPanel.Children.Add(noButton);
        buttonPanel.Children.Add(yesButton);
        content.Children.Add(buttonPanel);

        dialog.Content = content;

        await dialog.ShowDialog(parent);
        return result;
    }
}