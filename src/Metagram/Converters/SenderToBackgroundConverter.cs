using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using System.Globalization;
using Color = Avalonia.Media.Color;

namespace Metagram.Converters;

public class SenderToBackgroundConverter : MarkupExtension, IValueConverter
{
    private static readonly Brush ServiceMessageBackgroundBrush = new SolidColorBrush(Colors.Gray);
    private static readonly Brush MyMessageBackgroundBrush = new SolidColorBrush(Color.FromRgb(0x2b, 0x52, 0x78)); // #2b5278
    private static readonly Brush OtherMessageBackgroundBrush = new SolidColorBrush(Color.FromRgb(0x18, 0x25, 0x33)); // #182533

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null || value is not User from)
            return ServiceMessageBackgroundBrush;

        return from.IsBot ? MyMessageBackgroundBrush : OtherMessageBackgroundBrush;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => AvaloniaProperty.UnsetValue;
    public override object ProvideValue(IServiceProvider serviceProvider) => this;
}
