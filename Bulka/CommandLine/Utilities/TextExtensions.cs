using System;
using System.Globalization;
using System.Text;
using UnityEngine;

namespace Bulka.CommandLine.Utilities
{
    [CLSCompliant(false)]
    public static class TextExtensions
    {
        public static int HexToDec(this char hex)
        {
            if ('0' <= hex && hex <= '9') return hex - '0';
            if ('a' <= hex && hex <= 'f') return 10 + hex - 'a';
            if ('A' <= hex && hex <= 'F') return 10 + hex - 'A';
            throw new AssertException(hex.ToString());
        }
        public static int HexToDec(this char hex0, char hex1)
        {
            return hex0.HexToDec() * 16 + hex1.HexToDec();
        }
        public static char DecToHex(this int value, char startLetter = 'a')
        {
            if (0 <= value && value <= 9) return (char) ('0' + value);
            if (10 <= value && value <= 15) return (char) (startLetter + value - 10);
            throw new AssertException(value.ToString());
        }
        public static void DecToHex(this int value, out char hex0, out char hex1, char startLetter = 'a')
        {
            if (value < 0 || value > 255)
                throw new Exception(value.ToString());
            hex0 = (value >> 4).DecToHex(startLetter);
            hex1 = (value & 0xf).DecToHex(startLetter);
        }
        public static string ToHex(this Color color, bool alpha = false)
        {
            var r = Mathf.RoundToInt(color.r * 255);
            var g = Mathf.RoundToInt(color.g * 255);
            var b = Mathf.RoundToInt(color.b * 255);
            var a = Mathf.RoundToInt(color.a * 255);

            r.DecToHex(out var r0, out var r1);
            g.DecToHex(out var g0, out var g1);
            b.DecToHex(out var b0, out var b1);
            a.DecToHex(out var a0, out var a1);

            var canBeShort = r0 == r1 && g0 == g1 && b0 == b1 && (!alpha || a0 == a1);

            var rgb = canBeShort ? "" + r0 + g0 + b0 : "" + r0 + r1 + g0 + g1 + b0 + b1;
            return alpha || a != 255 ? rgb + (canBeShort ? "" + a0 : "" + a0 + a1) : rgb;
        }

        public static Color ParseHexColor(this string text)
        {
            var offset = text[0] == '#' ? 1 : 0;
            var length = text.Length - offset;
            switch (length)
            {
                case 3:
                case 4:
                {
                    var r = text[offset].HexToDec(text[offset]);
                    var g = text[offset + 1].HexToDec(text[offset + 1]);
                    var b = text[offset + 2].HexToDec(text[offset + 2]);
                    var a = length == 4 ? text[offset + 3].HexToDec(text[offset + 3]) : 255;
                    return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
                }
                case 6:
                case 8:
                {
                    var r = text[offset].HexToDec(text[offset + 1]);
                    var g = text[offset + 2].HexToDec(text[offset + 3]);
                    var b = text[offset + 4].HexToDec(text[offset + 5]);
                    var a = length == 8 ? text[offset + 6].HexToDec(text[offset + 7]) : 255;
                    return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
                }
                default:
                    throw new AssertException(text);
            }
        }

        public static bool ParseBoolean(this string text)
        {
            switch (text.ToUpperInvariant())
            {
                case "TRUE":
                case "YES":
                case "ON":
                case "T":
                case "Y":
                case "+":
                case "1":
                    return true;
                case "FALSE":
                case "NO":
                case "OFF":
                case "F":
                case "N":
                case "-":
                case "0":
                    return false;
                default:
                    throw new Exception(text);
            }
        }

        public static int ParseInteger(this string text)
        {
            return int.Parse(text);
        }

        public static float ParseFloat(this string text)
        {
            return float.Parse(text, CultureInfo.InvariantCulture);
        }

        public static string ParseString(this string text)
        {
            (text[0] == '"').Assert(text[0].ToString);
            var sb = new StringBuilder();
            for (var i = 1; i < text.Length - 1;)
                switch (text[i])
                {
                    case '\\':
                        switch (text[i + 1])
                        {
                            case '\\':
                                sb.Append('\\');
                                break;
                            case 'r':
                                sb.Append('\r');
                                break;
                            case 'n':
                                sb.Append('\n');
                                break;
                            case 't':
                                sb.Append('\t');
                                break;
                            case '"':
                                sb.Append('"');
                                break;
                            default:
                                throw new AssertException(text[i + 1].ToString());
                        }
                        i += 2;
                        break;
                    default:
                        sb.Append(text[i++]);
                        break;
                }
            return sb.ToString();
        }

        public static string Format(this bool value)
        {
            return value ? "true" : "false";
        }
        public static string Format(this int value)
        {
            return value.ToString();
        }
        public static string Format(this float value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }
        public static string Format(this string text)
        {
            var sb = new StringBuilder("\"");
            foreach (var c in text)
                switch (c)
                {
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    case '"':
                        sb.Append("\\\"");
                        break;
                    default:
                        sb.Append(c);

                        break;
                }
            sb.Append('"');
            return sb.ToString();
        }
        public static string Format(this Color color)
        {
            return "#" + color.ToHex();
        }
        public static string Format(this object value)
        {
            return value switch
            {
                bool booleanValue  => booleanValue.Format(),
                int integerValue   => integerValue.Format(),
                float floatValue   => floatValue.Format(),
                string stringValue => stringValue.Format(),
                Color colorValue   => colorValue.Format(),
                _                  => throw new NotImplementedException(value.GetType().ToString())
            };
        }
    }
}