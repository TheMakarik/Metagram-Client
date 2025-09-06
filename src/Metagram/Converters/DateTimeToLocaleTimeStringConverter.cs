﻿using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Metagram.Converters;

[ValueConversion(typeof(DateTime), typeof(string))]
public class DateTimeToLocaleTimeStringConverter : MarkupExtension, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not DateTime date)
            throw new ArgumentException("value must contain a DateTime", nameof(value));

        return date.ToString("t", CultureInfo.CurrentCulture);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => DependencyProperty.UnsetValue;
    public override object ProvideValue(IServiceProvider serviceProvider) => this;
}
