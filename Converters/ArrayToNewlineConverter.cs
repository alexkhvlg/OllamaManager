using System.Globalization;
using System.Windows.Data;

namespace OllamaManager.Converters;

public class ArrayToNewlineConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string[] array)
        {
            return string.Join(Environment.NewLine, array);
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string str)
        {
            return str.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        }
        return value;
    }
}
