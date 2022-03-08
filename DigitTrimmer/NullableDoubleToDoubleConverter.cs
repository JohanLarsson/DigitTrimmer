namespace DigitTrimmer
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    [ValueConversion(typeof(double?), typeof(double))]
    public class NullableDoubleToDoubleConverter : IValueConverter
    {
        public static readonly NullableDoubleToDoubleConverter Default = new NullableDoubleToDoubleConverter();

        private NullableDoubleToDoubleConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double?)value ?? 100;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException($"{nameof(NullableDoubleToDoubleConverter)} can only be used in OneWay bindings");
        }
    }
}
