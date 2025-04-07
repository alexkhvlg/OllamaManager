using System.Globalization;

namespace OllamaManager.Helpers;

public static class ParameterSizeToDecimal
{
    public static decimal Convert(string? size)
    {
        if (string.IsNullOrWhiteSpace(size))
        {
            return 0;
        }

        // Удаляем пробелы и приводим строку к нижнему регистру
        size = size.Trim().ToLower();

        // Определяем множитель в зависимости от суффикса
        decimal multiplier = 1;
        if (size.EndsWith("k"))
        {
            multiplier = 1_000; // Тысячи
            size = size.Substring(0, size.Length - 1); // Удаляем суффикс
        }
        else if (size.EndsWith("m"))
        {
            multiplier = 1_000_000; // Мегабайты
            size = size.Substring(0, size.Length - 1); // Удаляем суффикс
        }
        else if (size.EndsWith("b"))
        {
            multiplier = 1_000_000_000; // Миллиарды
            size = size.Substring(0, size.Length - 1); // Удаляем суффикс
        }

        // Преобразуем оставшуюся часть строки в число
        if (decimal.TryParse(size, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal number))
        {
            return number * multiplier; // Умножаем на множитель
        }
        else
        {
            return 0;
        }
    }
}
