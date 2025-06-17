using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Data.Converters;
using Avalonia.Media;
using LearnAvalonia;
using LearnAvalonia.Models;

namespace LearnAvalonia.Converters
{
    public class PriorityToBrushConverter : IValueConverter
    {
        // Custom brush colours
        private static readonly SolidColorBrush CompleteBrush = new(Color.FromRgb(155, 251, 146));
        private static readonly SolidColorBrush LowPrioBrush = new(Color.FromRgb(230, 224, 33));
        private static readonly SolidColorBrush MediumPrioBrush = new(Color.FromRgb(255, 200, 50));
        private static readonly SolidColorBrush HighPrioBrush = new(Color.FromRgb(255, 68, 0));
        private static readonly SolidColorBrush CriticalPrioBrush = new(Color.FromRgb(255, 0, 0));

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Priority priority)
            {
                return priority switch
                {
                    Priority.Complete => CompleteBrush,
                    Priority.Low => LowPrioBrush,
                    Priority.Medium => MediumPrioBrush,
                    Priority.High => HighPrioBrush,
                    Priority.Critical => CriticalPrioBrush,
                    _ => Brushes.Gray,
                };
            }

            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
