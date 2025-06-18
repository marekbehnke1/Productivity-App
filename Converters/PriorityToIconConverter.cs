using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Avalonia.Svg.Skia;
using LearnAvalonia;
using LearnAvalonia.Models;

namespace LearnAvalonia.Converters
{
    public class PriorityToIconConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is Priority priority)
            {
                return priority switch
                {
                    Priority.Complete => new SvgImage { Source = SvgSource.Load("avares://LearnAvalonia/Assets/Images/complete.svg") },
                    Priority.High => new SvgImage { Source = SvgSource.Load("avares://LearnAvalonia/Assets/Images/priorityHigh.svg") },
                    Priority.Critical => new SvgImage { Source = SvgSource.Load("avares://LearnAvalonia/Assets/Images/priorityCritical.svg") },
                    _ => new SvgImage {  },
                };
            }

            // return something empty
            return new SvgImage {  };
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
