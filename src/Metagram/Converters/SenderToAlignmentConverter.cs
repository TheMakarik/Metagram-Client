namespace Metagram.Converters;

public class SenderToAlignmentConverter : MarkupExtension, IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null || value is not User from)
            return HorizontalAlignment.Center;

        return from.IsBot ? HorizontalAlignment.Right : HorizontalAlignment.Left;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => AvaloniaProperty.UnsetValue;
    public override object ProvideValue(IServiceProvider serviceProvider) => this;
}
