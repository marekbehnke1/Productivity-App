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

namespace LearnAvalonia;

public partial class MainView : Window
{

    private bool TimerRunning { get; set; } = false;
    private System.Timers.Timer? animTimer;
    private CancellationTokenSource? animationCts;
    private List<Button> NavButtons { get; set; }
    public MainView()
    {
        InitializeComponent();

        // Create List of UI nav buttons
        NavButtons = new List<Button>
        {
            AllTasksBtn,
            CritPrioTasksBtn,
            HighPrioTasksBtn,
            MedPrioTasksBtn,
            LowPrioTasksBtn,
            CompletedTasksBtn
        };
    }
   
    // Registering Collapse Property
    public static readonly StyledProperty<bool> IsCollapsedProperty =
        AvaloniaProperty.Register<MainView, bool>(nameof(IsCollapsed));

    public bool IsCollapsed
    {
        get => GetValue(IsCollapsedProperty);
        set => SetValue(IsCollapsedProperty, value);
    }

    async private void Timer_Start(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var animation = (Animation?)this.Resources["TimerAnimationForward"];
        var animationReverse = (Animation?)this.Resources["TimerAnimationReverse"];

        if (TimerRunning)
        {
            // set timer bool
            TimerRunning = false;

            //stop the animation
            animationCts?.Cancel();
            animationCts?.Dispose();

            //stop the timer
            animTimer?.Stop();
            animTimer?.Dispose();

            //reset ui values
            TimerButton.Content = "Start";
            TimerBar.Value = 0;
            TimerText.Text = "00:00:00";

            return;
        }
        // initialise timer and animation values
        TimerButton.Content = "Stop";
        TimerRunning = true;

        animationCts = new CancellationTokenSource();

        try
        {
            if (animation != null) {
                TimerText.Text = "25:00:00";
                // Set initial timer values
                TimeSpan animTime = animation.Duration;
                DateTime animEnd = DateTime.Now + animTime;

                animTimer = new System.Timers.Timer(1000);

                //init timer and timer event
                animTimer.Elapsed += new ElapsedEventHandler((sender, e) => AnimTimerEvent(animTimer, e, animEnd));
                animTimer.AutoReset = true;
                animTimer.Start();

                await animation.RunAsync(TimerBar, animationCts.Token);

                // stop timer once animation has finished
                animTimer.Stop();

                TimerBar.FlowDirection = Avalonia.Media.FlowDirection.RightToLeft;
            }

            if (animationReverse != null && TimerRunning == true)
            {
                // Set initial timer values
                TimeSpan animTime = animationReverse.Duration;
                DateTime animEnd = DateTime.Now + animTime;


                // create a new timer
                animTimer = new System.Timers.Timer(1000);

                // init timer and timer event
                animTimer.Elapsed += new ElapsedEventHandler((sender, e) => AnimTimerEvent(animTimer, e, animEnd));
                animTimer.AutoReset = true;
                animTimer.Start();

                await animationReverse.RunAsync(TimerBar, animationCts.Token);

                // stop timer once animation has finished
                animTimer.Stop();

                TimerBar.FlowDirection = Avalonia.Media.FlowDirection.LeftToRight;
                TimerText.Text = "Start";
            }

        }
        catch (OperationCanceledException)
        {
            // cancelled operation caught
        }
        finally
        {

            // reset everything 
            TimerRunning = false;
            animationCts?.Dispose();
            animTimer?.Dispose();
            TimerBar.FlowDirection = Avalonia.Media.FlowDirection.LeftToRight;
        }
    }

    void AnimTimerEvent(Object source, ElapsedEventArgs e, DateTime animEnd)
    {
        TimeSpan animRemaining = animEnd - DateTime.Now;

        // think this sets a default
        string text = animRemaining > TimeSpan.Zero ? animRemaining.ToString(@"hh\:mm\:ss") : "00:00:00";

        Dispatcher.UIThread.Post(() =>
        {
            TimerText.Text = text;
        });
    }

    private void CreateNote(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {

        if (DataContext is MainViewModel viewModel)
        {
            viewModel.AddNewTask();
            // Scrolls to the end when you add a new note
            // NotesScroller.ScrollToEnd();
        }
    }

    private async void CollapseApp(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        IsCollapsed = !IsCollapsed;
        System.Diagnostics.Debug.WriteLine($"IsCollapsed is now: {IsCollapsed}");

        var collapseAnim = (Animation?)Resources["WindowCollapse"];
        var expandAnim = (Animation?)Resources["WindowExpand"];

        if (collapseAnim != null && IsCollapsed)
        {
            MainPanel.MaxHeight = 0;
            CarouselNavBar.Height = 0;
            await collapseAnim.RunAsync(this);

            //System.Diagnostics.Debug.WriteLine($"MainPanel height is: {MainPanel.Height}");
            //System.Diagnostics.Debug.WriteLine($"CarouselNavBar height is: {CarouselNavBar.Height}");
        }
        else if (expandAnim != null && !IsCollapsed)
        {
            await expandAnim.RunAsync(this);
            MainPanel.MaxHeight = 700;
            CarouselNavBar.Height = 30;

            //System.Diagnostics.Debug.WriteLine($"MainPanel height is: {MainPanel.Height}");
            //System.Diagnostics.Debug.WriteLine($"CarouselNavBar height is: {CarouselNavBar.Height}");


        }

    }
    public void Previous(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (DataContext is MainViewModel viewModel)
        {
            GotoPanelIndex(viewModel.CurrentPanelIndex - 1, sender);
            System.Diagnostics.Debug.WriteLine($"Current index is: {viewModel.CurrentPanelIndex}");
        }
    }

    public void Next(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if(DataContext  is MainViewModel viewModel)
        {
            GotoPanelIndex(viewModel.CurrentPanelIndex + 1, sender);
            System.Diagnostics.Debug.WriteLine($"Current index is: {viewModel.CurrentPanelIndex}");
        }
    }
    public void GotoPanelIndex(int index, object? sender)
    {
        if (DataContext is MainViewModel viewModel)
        {
            // Check panel index is within range
            if (index >= 0 && index < NotesCarousel.ItemCount)
            {
                viewModel.CurrentPanelIndex = index;
            }
            else if(index < 0)
            {
                viewModel.CurrentPanelIndex = NotesCarousel.ItemCount - 1;
            }
            else if(index > NotesCarousel.ItemCount - 1)
            {
                viewModel.CurrentPanelIndex = 0;
            }

            // Highlight the current panels button
            if (sender?.GetType() == typeof(Button))
            {
                foreach (Button item in NavButtons)
                {
                    // TODO: Fix this colour - colour nearly spot on now - think its a touch too dark. maybe.
                    item.Background = AppBrushes.NavButtonBackground;
                }
                NavButtons[viewModel.CurrentPanelIndex].Background = new SolidColorBrush(Colors.Gray);
            }
        }

    }

    private void GotoAllTasks(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        GotoPanelIndex(0, sender);

    }
    private void GotoCriticalTasks(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        GotoPanelIndex(1, sender);
    }

    private void GotoHighTasks(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        GotoPanelIndex(2, sender);
    }

    private void GotoMediumTasks(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        GotoPanelIndex(3, sender);
    }

    private void GotoLowTasks(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        GotoPanelIndex(4, sender);
    }

    private void GotoCompletedTasks(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        GotoPanelIndex(5, sender);
    }
}