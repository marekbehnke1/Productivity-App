using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;

namespace LearnAvalonia;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

    }

    async private void Timer_Start(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        var animation = (Animation)this.Resources["TimerAnimationForward"];
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                              // Running XAML animation on the Rect control. 
        await animation.RunAsync(TimerBar);
        TimerBar.FlowDirection = Avalonia.Media.FlowDirection.RightToLeft;

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        var animationReverse = (Animation)this.Resources["TimerAnimationReverse"];
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                              // Running XAML animation on the Rect control. 
        await animationReverse.RunAsync(TimerBar);
        TimerBar.FlowDirection = Avalonia.Media.FlowDirection.LeftToRight;
    }
}