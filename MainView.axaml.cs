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

namespace LearnAvalonia;

public partial class MainView : Window
{
    public MainView()
    {
        InitializeComponent();
    }

    private bool TimerRunning { get; set; } = false;
    private System.Timers.Timer? animTimer;
    private CancellationTokenSource? animationCts;

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
        NotesPanel.Children.Add(new ListItem());

        // Scrolls to the end when you add a new note
        NotesScroller.ScrollToEnd();
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
            await collapseAnim.RunAsync(this);
        }
        else if (expandAnim != null && !IsCollapsed)
        {
            await expandAnim.RunAsync(this);
            MainPanel.MaxHeight = 700;


        }

    }
}