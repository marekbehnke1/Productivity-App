using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using LearnAvalonia.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;

namespace LearnAvalonia;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // This doesnt currently seem to be doing anything?
            desktop.MainWindow = new MainView
            {
                DataContext = new MainViewModel()
            };
            
        }

        base.OnFrameworkInitializationCompleted();
    }
}