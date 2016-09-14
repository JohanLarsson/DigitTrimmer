﻿namespace DigitTrimmer
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Markup;
    using System.Windows.Media;

    public class AreEqualToBrushConverter : MarkupExtension, IMultiValueConverter
    {
        public Brush WhenEqual { get; set; }

        public Brush WhenNot { get; set; }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
            {
                throw new ArgumentException("Can only be used with two values.");
            }

            return Equals(values[0], values[1])
                       ? this.WhenEqual
                       : this.WhenNot;
        }

        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
