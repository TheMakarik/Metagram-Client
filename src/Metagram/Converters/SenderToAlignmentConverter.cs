using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Metagram.Converters;

[ValueConversion(typeof(Message), typeof(HorizontalAlignment))]
public class SenderToAlignmentConverter : MarkupExtension, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Message message)
            throw new ArgumentException(nameof(value));

        if (message.From is not { Id: > 0 } from)
            return HorizontalAlignment.Center;

        return from.IsBot ? HorizontalAlignment.Right : HorizontalAlignment.Left;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => DependencyProperty.UnsetValue;
    public override object ProvideValue(IServiceProvider serviceProvider) => this;
}
