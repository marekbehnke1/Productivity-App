using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using LearnAvalonia.Resources;


namespace LearnAvalonia.Converters
{
    public class StatusMessageColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isSuccess)
            {
                return isSuccess ? AppBrushes.SuccessMessage : AppBrushes.ErrorMessage;
            }

            return AppBrushes.ErrorMessage;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
