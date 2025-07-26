using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using LearnAvalonia.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using LearnAvalonia.Services;
using System;
using System.Net.Http;

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

        services.AddSingleton<IAuthenticationService>(provider =>
        {
            var httpClient = new HttpClient
            {
                //BaseAddress = new Uri("https://localhost:7116/")
                BaseAddress = new Uri("https://notesapp-api-mb-htgkhxg7dxfpbdfd.canadacentral-01.azurewebsites.net/")
            };
            return new AuthenticationService(httpClient);
        });

        // Add the http auth handler to the list of services
        services.AddTransient<AuthenticationHandler>();

        // Register the TaskService <- Local task service
        //services.AddSingleton<ITaskService, TaskService>();

        //Register the API task service
        services.AddHttpClient<ITaskService, ApiTaskService>(client =>
        {
            // URL of your API
            //client.BaseAddress = new Uri("https://localhost:7116/");
            client.BaseAddress = new Uri("https://notesapp-api-mb-htgkhxg7dxfpbdfd.canadacentral-01.azurewebsites.net/");

        }).AddHttpMessageHandler<AuthenticationHandler>();

        //Register the MainViewModel
        services.AddTransient<MainViewModel>();

        // This builds the services defined above into the ServiceProvider property.
        ServiceProvider = services.BuildServiceProvider();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Get all services from DI

            var mainViewModel = ServiceProvider?.GetRequiredService<MainViewModel>();
            var authService = ServiceProvider?.GetRequiredService<IAuthenticationService>();
            var taskService = ServiceProvider?.GetRequiredService<ITaskService>();

            var navigationViewModel = new NavigationViewModel(authService!, taskService!, mainViewModel!);

            desktop.MainWindow = new MainView
            {
                DataContext = navigationViewModel
            };

            //Fire and forget auth initialisation
            // Will have to change this when we are using secure storage
            //_ = authService?.InitializeAuthAsync();
            
        }

        base.OnFrameworkInitializationCompleted();
    }
}