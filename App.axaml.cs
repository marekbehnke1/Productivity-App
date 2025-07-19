using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using LearnAvalonia.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using LearnAvalonia.Services;
using System;

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

        // Configure the http client for auth service
        services.AddHttpClient<IAuthenticationService, AuthenticationService>(client =>
        {
            client.BaseAddress = new Uri("https://localhost:7116");
        });

        // Register the TaskService <- Local task service
        //services.AddSingleton<ITaskService, TaskService>();

        // Add the http auth handler to the list of services
        services.AddTransient<AuthenticationHandler>();

        //Register the HTTPService <- API task service
        services.AddHttpClient<ITaskService, ApiTaskService>(client =>
        {
            // URL of your API
            client.BaseAddress = new Uri("https://localhost:7116/");
        })
            // Adding the http service here, ensures that the token in injected into all of the requests the task service sends
            .AddHttpMessageHandler<AuthenticationHandler>();

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

            //Fire and forget auth initialisation
            // Will have to change this when we are using secure storage
            var authService = ServiceProvider?.GetRequiredService<IAuthenticationService>();
            _ = authService?.InitializeAuthAsync();
            
        }

        base.OnFrameworkInitializationCompleted();
    }
}