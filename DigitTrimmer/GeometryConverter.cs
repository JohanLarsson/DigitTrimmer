namespace DigitTrimmer
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Windows.Media;

    public static class GeometryConverter
    {
        private static readonly Regex DoubleRegex = new Regex(@"(-?\d+(?:\.\d+)?(?:E-?\d+)?)");

        public static string RoundDigits(string text, int digits)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            var format = digits == 0
                                ? "0"
                                : $"0.{new string('#', digits)}";

            var replace = DoubleRegex.Replace(text, m =>
            {
                var d = double.Parse(m.Value, CultureInfo.InvariantCulture);
                return d.ToString(format, CultureInfo.InvariantCulture);
            });

            return replace;
        }

        public static string ShiftToOrigin(string text)
        {
            var geometry = Geometry.Parse(text);
            var topLeft = geometry.Bounds.TopLeft;
            var transform = new TranslateTransform(-topLeft.X, -topLeft.Y);
            var combined = Geometry.Combine(geometry, geometry, GeometryCombineMode.Intersect, transform);
            return combined.ToString(CultureInfo.InvariantCulture);
        }

        public static string WithSize(string text, double size)
        {
            var geometry = Geometry.Parse(text);
            var topLeft = geometry.Bounds.TopLeft;
            var currentSize = Math.Max(geometry.Bounds.Width, geometry.Bounds.Height);
            var transform = new ScaleTransform(size / currentSize, size / currentSize, topLeft.X, topLeft.Y);
            var combined = Geometry.Combine(geometry, geometry, GeometryCombineMode.Intersect, transform);
            return combined.ToString(CultureInfo.InvariantCulture);
        }
    }
}
