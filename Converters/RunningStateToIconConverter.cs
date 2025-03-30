using System.Globalization;
using System.Windows.Data;

namespace OllamaManager.Converters;

public class RunningStateToIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isRunning && isRunning)
        {
            // Можно использовать любую другую иконку
            return "🚀";
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}