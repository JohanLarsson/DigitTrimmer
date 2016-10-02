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

        public static string ShiftToTopLeft(string text, double margin, double tolerance = 0.001, ToleranceType toleranceType = ToleranceType.Absolute)
        {
            var geometry = Geometry.Parse(text);
            var topLeft = geometry.Bounds.TopLeft;
            var transform = new TranslateTransform(-topLeft.X + margin, -topLeft.Y + margin);
            return WithTransform(geometry, transform, tolerance, toleranceType).ToString(CultureInfo.InvariantCulture);
        }

        public static string ShiftToCenter(string text, double size, double tolerance = 0.001, ToleranceType toleranceType = ToleranceType.Absolute)
        {
            var geometry = Geometry.Parse(text);
            var centerX = geometry.Bounds.TopLeft.X - geometry.Bounds.BottomRight.X;
            var centerY = geometry.Bounds.TopLeft.Y - geometry.Bounds.BottomRight.Y;
            var transform = new TranslateTransform(size - centerX, size - centerY);
            return WithTransform(geometry, transform, tolerance, toleranceType).ToString(CultureInfo.InvariantCulture);
        }

        public static string WithSize(string text, double size, double tolerance = 0.001, ToleranceType toleranceType = ToleranceType.Absolute)
        {
            var geometry = Geometry.Parse(text);
            var topLeft = geometry.Bounds.TopLeft;
            var currentSize = Math.Max(geometry.Bounds.Width, geometry.Bounds.Height);
            var transform = new ScaleTransform(size / currentSize, size / currentSize, topLeft.X, topLeft.Y);
            return WithTransform(geometry, transform, tolerance, toleranceType).ToString(CultureInfo.InvariantCulture);
        }

        private static Geometry WithTransform(Geometry geometry, Transform transform, double tolerance = 0.001, ToleranceType toleranceType = ToleranceType.Absolute)
        {
            var combined = Geometry.Combine(geometry, geometry, GeometryCombineMode.Intersect, transform, tolerance, toleranceType);
            return combined;
        }
    }
}
