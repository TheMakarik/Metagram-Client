using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using System.Globalization;

namespace Metagram.Converters;

public class UserInfoToStringConverter : MarkupExtension, IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not User user)
            throw new ArgumentException("", nameof(value));

        return user.ToDisplayString() ?? string.Empty;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => AvaloniaProperty.UnsetValue;
    public override object ProvideValue(IServiceProvider serviceProvider) => this;
}
