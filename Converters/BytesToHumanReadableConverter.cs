using System.Globalization;
using System.Windows.Data;
using OllamaManager.Helpers;

namespace OllamaManager.Converters;

public class BytesToHumanReadableConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is long byteSize)
        {
            return BytesToHumanReadableFormat.Convert(byteSize);
        }
        return "0 B";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}