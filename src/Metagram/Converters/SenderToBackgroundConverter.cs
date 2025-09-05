using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using Color = System.Windows.Media.Color;
using Message = Telegram.Bot.Types.Message;

namespace Metagram.Converters;

[ValueConversion(typeof(Message), typeof(Brush))]
public class SenderToBackgroundConverter : MarkupExtension, IValueConverter
{
    private static readonly Brush ServiceMessageBackgroundBrush = new SolidColorBrush(Colors.Gray);
    private static readonly Brush MyMessageBackgroundBrush = new SolidColorBrush(Color.FromRgb(0x2b, 0x52, 0x78)); // #2b5278
    private static readonly Brush OtherMessageBackgroundBrush = new SolidColorBrush(Color.FromRgb(0x18, 0x25, 0x33)); // #182533

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Message message)
            throw new ArgumentException("Should be Message", nameof(value));

        if (message.From is not { Id: > 0 } from)
            return ServiceMessageBackgroundBrush;

        return from.IsBot ? MyMessageBackgroundBrush : OtherMessageBackgroundBrush;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => DependencyProperty.UnsetValue;
    public override object ProvideValue(IServiceProvider serviceProvider) => this;
}
