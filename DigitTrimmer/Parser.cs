namespace DigitTrimmer
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Windows;

    public static class Parser
    {
        private static readonly Regex DoubleRegex = new Regex(@"-?\d+(?:\.\d+)?", RegexOptions.ExplicitCapture);

        private static readonly Regex PointRegex = new Regex($@"(?<x>{DoubleRegex}),(?<y>{DoubleRegex})", RegexOptions.ExplicitCapture);

        public static IEnumerable<object> GetTokens(string geometry)
        {
            var pos = 0;
            SkipWhitespace(geometry, ref pos);
            while (pos < geometry.Length)
            {
                if (TryParseToken(geometry, ref pos, out var token))
                {
                    yield return token;
                    SkipWhitespace(geometry, ref pos);
                    continue;
                }

                if (TryParsePoint(geometry, ref pos, out var point))
                {
                    yield return point;
                    SkipWhitespace(geometry, ref pos);
                    continue;
                }

                if (TryParseDouble(geometry, ref pos, out var value))
                {
                    yield return value;
                    SkipWhitespace(geometry, ref pos);
                    continue;
                }

                throw new FormatException();
            }

            if (pos != geometry.Length)
            {
                throw new FormatException();
            }
        }

        private static bool TryParseToken(string geometry, ref int pos, [NotNullWhen(true)]out string? token)
        {
            SkipWhitespace(geometry, ref pos);
            if (pos < geometry.Length && char.IsLetter(geometry[pos]))
            {
                token = new string(geometry[pos], 1);
                pos++;
                if (TryParseToken(geometry, ref pos, out _))
                {
                    throw new FormatException($"two tokens in a row at position {pos} in string {geometry}");
                }

                return true;
            }

            token = null;
            return false;
        }

        private static bool TryParsePoint(string geometry, ref int pos, out Point point)
        {
            SkipWhitespace(geometry, ref pos);
            if (pos < geometry.Length && PointRegex.IsMatch(geometry, pos))
            {
                var match = PointRegex.Match(geometry, pos);
                point = new Point(double.Parse(match.Groups["x"].Value, CultureInfo.InvariantCulture), double.Parse(match.Groups["y"].Value, CultureInfo.InvariantCulture));
                pos += match.Length;
                return true;
            }

            point = new Point(0, 0);
            return false;
        }

        private static bool TryParseDouble(string geometry, ref int pos, out double value)
        {
            SkipWhitespace(geometry, ref pos);
            if (pos < geometry.Length && DoubleRegex.IsMatch(geometry, pos))
            {
                var match = DoubleRegex.Match(geometry, pos);
                value = double.Parse(match.Value, CultureInfo.InvariantCulture);
                pos += match.Length;
                return true;
            }

            value = 0;
            return false;
        }

        private static void SkipWhitespace(string geometry, ref int pos)
        {
            while (pos < geometry.Length && char.IsWhiteSpace(geometry[pos]))
            {
                pos++;
            }
        }
    }
}
