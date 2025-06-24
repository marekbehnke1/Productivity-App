using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using LearnAvalonia.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using LearnAvalonia.Services;

namespace LearnAvalonia;

public partial class App : Application
{
    // Setup Service provider for DI
    public static ServiceProvider? ServiceProvider { get; private set; }
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

        // Configure services when app launches
        ConfigureServices();
    }

    private void ConfigureServices()
    {
        var services = new ServiceCollection();

        // Register the TaskService
        services.AddSingleton<ITaskService, TaskService>();

        //Register the MainViewModel
        services.AddTransient<MainViewModel>();

        // This builds the services defined above into the ServiceProvider property.
        ServiceProvider = services.BuildServiceProvider();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Get the MainViewModel from the service provider instead of declaring it manually
            var mainViewModel = ServiceProvider?.GetRequiredService<MainViewModel>();

            desktop.MainWindow = new MainView
            {
                DataContext = mainViewModel
            };
            
        }

        base.OnFrameworkInitializationCompleted();
    }
}