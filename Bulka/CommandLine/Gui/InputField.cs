using System;
using Bulka.CommandLine.Commands;
using UnityEngine;

namespace Bulka.CommandLine.Gui
{
    [CLSCompliant(false)]
    public class InputField
    {
        public string Text { get; private set; } = "";
        private string _coloredText = "";

        public void SetText(string text, SyntaxHighlighter syntaxHighlighter, Func<string, Command> getCommand)
        {
            Text         = text;
            _coloredText = syntaxHighlighter.Colorize(text, getCommand);
        }

        public string Draw(Rect rect, GUIStyle inputStyle, GUIStyle coloredInputStyle)
        {
            var newText = GUI.TextField(rect, Text, inputStyle);
            GUI.Label(rect, _coloredText, coloredInputStyle);
            return newText;
        }
    }
}