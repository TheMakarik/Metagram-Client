using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using System.Globalization;

namespace Metagram.Converters;

public class DateTimeToLocaleTimeStringConverter : MarkupExtension, IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not DateTime date)
            throw new ArgumentException("value must contain a DateTime", nameof(value));

        return date.ToString("t", CultureInfo.CurrentCulture);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => AvaloniaProperty.UnsetValue;
    public override object ProvideValue(IServiceProvider serviceProvider) => this;
}
