using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using System.Globalization;
using System.IO;

namespace Metagram.Converters;

public class FileIdToBitmapConverter : MarkupExtension, IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => AvaloniaProperty.UnsetValue;
    public override object ProvideValue(IServiceProvider serviceProvider) => this;
}
