using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime;
using Bulka.CommandLine.Commands;
using Bulka.CommandLine.Utilities;
using UnityEngine;

namespace Bulka.CommandLine.Gui
{
    [CLSCompliant(false)]
    public class SyntaxHighlighter
    {
        public const int unknownToken = 0;

        public Color FunctionColor { get; set; } = Dracula.green;
        public Color VariableColor { get; set; } = Dracula.purple;
        public Color UnknownCommandColor { get; set; } = Dracula.foreground;
        public Color UnknownTokenColor { get; set; } = Dracula.foreground;
        public Color WordColor { get; set; } = Dracula.foreground;
        public Color DoubleQuotedStringColor { get; set; } = Dracula.orange;
        public Color SingleQuotedStringColor { get; set; } = Dracula.orange;
        public Color RealColor { get; set; } = Dracula.purple;
        public Color IntegerColor { get; set; } = Dracula.cyan;
        public Color Int2Color { get; set; } = Dracula.green;
        public Color RGBColor { get; set; } = Dracula.green;
        public Color RgbaColor { get; set; } = Dracula.green;
        public Color BooleanColor { get; set; } = Dracula.green;

        private readonly CommandLineLexer _lexer;
        private readonly StringBuilder _sb = new StringBuilder();
        private readonly List<IToken> _tokens = new List<IToken>();
        private readonly List<(Color color, string text, int tokenType)> _parts = new List<(Color color, string text, int tokenType)>();

        public SyntaxHighlighter()
        {
            _lexer = new CommandLineLexer(null);
            _lexer.RemoveErrorListeners();
        }

        public Color GetTokenColor(IToken token)
        {
            return token.Type switch
            {
                CommandLineLexer.Word               => WordColor,
                CommandLineLexer.DoubleQuotedString => DoubleQuotedStringColor,
                CommandLineLexer.SingleQuotedString => SingleQuotedStringColor,
                CommandLineLexer.Real               => RealColor,
                CommandLineLexer.Integer            => IntegerColor,
                CommandLineLexer.ShortHexRgbColor   => token.Text.ParseHexColor(),
                CommandLineLexer.ShortHexRgbaColor  => token.Text.ParseHexColor(),
                CommandLineLexer.LongHexRgbColor    => token.Text.ParseHexColor(),
                CommandLineLexer.LongHexRgbaColor   => token.Text.ParseHexColor(),
                CommandLineLexer.Int2               => Int2Color,
                CommandLineLexer.Rgb                => RGBColor,
                CommandLineLexer.Rgba               => RgbaColor,
                CommandLineLexer.Boolean            => BooleanColor,
                _                                   => UnknownTokenColor
            };
        }

        public string Colorize(string text, Func<string, Command> getCommand = null)
        {
            _lexer.SetInputStream(new AntlrInputStream(text));

            _tokens.Clear();
            var t = _lexer.NextToken();
            while (t.Type != TokenConstants.EOF)
            {
                _tokens.Add(t);
                t = _lexer.NextToken();
            }

            _parts.Clear();
            if (_tokens.Count == 0)
                _parts.Add((UnknownTokenColor, text, unknownToken));

            else
            {
                _parts.Add((UnknownTokenColor, text.Substring(0, _tokens[0].StartIndex), unknownToken));
                _parts.Add((GetTokenColor(_tokens[0]), _tokens[0].Text, _tokens[0].Type));
                for (var i = 1; i < _tokens.Count; i++)
                {
                    _parts.Add((UnknownTokenColor, text.Substring(_tokens[i - 1].StopIndex + 1, _tokens[i].StartIndex - _tokens[i - 1].StopIndex - 1), unknownToken));
                    _parts.Add((GetTokenColor(_tokens[i]), _tokens[i].Text, _tokens[i].Type));
                }
                _parts.Add((UnknownTokenColor, text.Substring(_tokens[_tokens.Count - 1].StopIndex + 1, text.Length - _tokens[_tokens.Count - 1].StopIndex - 1), unknownToken));
            }

            if (getCommand != null)
                for (var i = 0; i < _parts.Count; i++)
                    if (_parts[i].tokenType != unknownToken && _parts[i].tokenType != CommandLineLexer.Whitespace)
                    {
                        if (_parts[i].tokenType == CommandLineLexer.Word)
                        {
                            Command command;
                            var color = _parts[i].tokenType != CommandLineLexer.Word || (command = getCommand(_parts[i].text)) == null
                                ? UnknownCommandColor
                                : command is ProcedureCommand
                                    ? FunctionColor
                                    : VariableColor;
                            _parts[i] = (color, _parts[i].text, _parts[i].tokenType);
                        }
                        break;
                    }

            _sb.Clear();
            foreach (var (color, text1, _) in _parts)
            {
                var hexColor = color.ToHex();
                foreach (var c in text1)
                    _sb.Append($"<color=#{hexColor}>{c}</color>");
            }
            return _sb.ToString();
        }
    }
}