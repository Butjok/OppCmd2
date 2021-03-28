using System;
using Bulka.CommandLine.Utilities;
using UnityEngine;

namespace Bulka.CommandLine
{
    [CLSCompliant(false)]
    public class Evaluator : CommandLineBaseVisitor<object>
    {
        public override object VisitBoolean(CommandLineParser.BooleanContext context)
        {
            return context.Boolean().GetText().ParseBoolean();
        }
        public override object VisitInteger(CommandLineParser.IntegerContext context)
        {
            return context.Integer().GetText().ParseInteger();
        }
        public override object VisitReal(CommandLineParser.RealContext context)
        {
            return context.Real().GetText().ParseFloat();
        }
        public override object VisitString(CommandLineParser.StringContext context)
        {
            return context.Word() != null ? context.Word().GetText() : context.GetText().ParseString();
        }
        public override object VisitInt2(CommandLineParser.Int2Context context)
        {
            return new Vector2Int((int) Visit(context.integer(0)), (int) Visit(context.integer(1)));
        }
        public override object VisitShortHexRgbColor(CommandLineParser.ShortHexRgbColorContext context)
        {
            return context.GetText().ParseHexColor();
        }
        public override object VisitShortHexRgbaColor(CommandLineParser.ShortHexRgbaColorContext context)
        {
            return context.GetText().ParseHexColor();
        }
        public override object VisitLongHexRgbColor(CommandLineParser.LongHexRgbColorContext context)
        {
            return context.GetText().ParseHexColor();
        }
        public override object VisitLongHexRgbaColor(CommandLineParser.LongHexRgbaColorContext context)
        {
            return context.GetText().ParseHexColor();
        }
        public override object VisitColorComponent(CommandLineParser.ColorComponentContext context)
        {
            return context.integer() != null ? (int) Visit(context.integer()) / 255f : Visit(context.real());
        }
        public override object VisitRgbColor(CommandLineParser.RgbColorContext context)
        {
            var r = (float) Visit(context.colorComponent(0));
            var g = (float) Visit(context.colorComponent(1));
            var b = (float) Visit(context.colorComponent(2));
            return new Color(r, g, b);
        }
        public override object VisitRgbaColor(CommandLineParser.RgbaColorContext context)
        {
            var r = (float) Visit(context.colorComponent(0));
            var g = (float) Visit(context.colorComponent(1));
            var b = (float) Visit(context.colorComponent(2));
            var a = (float) Visit(context.colorComponent(3));
            return new Color(r, g, b, a);
        }
    }
}