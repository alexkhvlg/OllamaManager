using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;

namespace OllamaManager.Converters;

public class BoolToCursorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is bool isLoading && isLoading ? Cursors.AppStarting : Cursors.Arrow;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
