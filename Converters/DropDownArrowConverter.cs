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
    public class DropDownArrowConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is true)
            {
                return new SvgImage { Source = SvgSource.Load("avares://LearnAvalonia/Assets/Images/arrowUp.svg") };
            }
            else
            {
                return new SvgImage { Source = SvgSource.Load("avares://LearnAvalonia/Assets/Images/arrowDown.svg") };
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
