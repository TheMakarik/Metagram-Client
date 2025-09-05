using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using Message = Telegram.Bot.Types.Message;

namespace Metagram.Converters;

[ValueConversion(typeof(LinkedListNode<Message>), typeof(HorizontalAlignment))]
public class MessageGroupIndexToCornerRadiusConverter : MarkupExtension, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not LinkedListNode<Message> node)
            throw new ArgumentException("should be Message", nameof(value));

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => DependencyProperty.UnsetValue;
    public override object ProvideValue(IServiceProvider serviceProvider) => this;
}
