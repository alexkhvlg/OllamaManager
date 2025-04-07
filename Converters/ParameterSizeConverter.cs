using System.Globalization;
using System.Windows.Data;

namespace OllamaManager.Converters;
public class ParameterSizeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not decimal number)
        {
            return value; // Возвращаем оригинальное значение, если оно не является числом
        }

        if (number >= 1_000_000_000)
        {
            return $"{number / 1_000_000_000m:0.##}B"; // Миллиарды
        }
        else if (number >= 1_000_000)
        {
            return $"{number / 1_000_000m:0.##}M"; // Миллионы
        }
        else if (number >= 1_000)
        {
            return $"{number / 1_000m:0.##}K"; // Тысячи
        }
        else
        {
            return number.ToString(); // Возвращаем число как строку, если оно меньше 1000
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException(); // Обратное преобразование не требуется
    }
}