using Metagram.Collections;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Metagram.Converters;

[ValueConversion(typeof(MessageNode), typeof(HorizontalAlignment))]
public class MessageGroupIndexToCornerRadiusConverter : MarkupExtension, IValueConverter
{
    private static readonly CornerRadius SingleRadius = new CornerRadius(10, 10, 10, 10);
    private static readonly CornerRadius MiddleRadius = new CornerRadius(0, 3, 3, 0);
    private static readonly CornerRadius BottomRadius = new CornerRadius(0, 3, 10, 0);
    private static readonly CornerRadius TopRadius = new CornerRadius(10, 3, 3, 0);

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not MessageNode node)
            throw new ArgumentException("should be Message", nameof(value));

        if (node.IsSingle)
            return SingleRadius;

        if (node.IsFirst)
            return TopRadius;

        if (node.IsLast)
            return BottomRadius;

        return MiddleRadius;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => DependencyProperty.UnsetValue;
    public override object ProvideValue(IServiceProvider serviceProvider) => this;
}
