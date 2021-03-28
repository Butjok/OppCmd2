using System;
using System.IO;
using Bulka.CommandLine.Commands;
using Bulka.CommandLine.Utilities;
using UnityEngine;

namespace Bulka.CommandLine.Gui
{
    [CLSCompliant(false)]
    public class ConsoleWindow : MonoBehaviour
    {
        [SerializeField] private string historyFilename = "History.txt";
        private readonly Bulka.CommandLine.CommandLine _commandLine = new Bulka.CommandLine.CommandLine {Debug = true};
        private History _history;
        [SerializeField] private string input = "";

        [SerializeField] private bool fly;

        private readonly InputField _inputField = new InputField();
        [SerializeField] private GUISkin skin;
        [SerializeField] private string inputStyleName = "ConsoleInput";
        [SerializeField] private string coloredInputStyleName = "ConsoleColoredInput";
        private GUIStyle _inputStyle;
        private GUIStyle _coloredInputStyle;
        private float _height;
        private readonly SyntaxHighlighter _syntaxHighlighter = new SyntaxHighlighter();

        private void Awake()
        {
            _commandLine.RegisterStaticCommands();

            _commandLine.AddCommand(
                "Test",
                new ActionProcedureCommand(_commandLine, new[] {"name"}, action: arguments => { Debug.Log(arguments[0]); }),
                new CommandMetaData(null, "Does nothing useful"));

            _commandLine.AddCommand("fly", new VirtualVariableCommand(_commandLine, () => fly, value => fly = (bool) value, true));

            _history = new History(Path.Combine(Application.persistentDataPath, historyFilename));

            ((bool) skin).Assert();

            _inputStyle = skin.FindStyle(inputStyleName);
            (_inputStyle != null).Assert();

            _coloredInputStyle = skin.FindStyle(coloredInputStyleName);
            (_coloredInputStyle != null).Assert();

            _height = _inputStyle.CalcSize(GUIContent.none).y;
        }

        private void OnApplicationQuit()
        {
            _history.Save(Path.Combine(Application.persistentDataPath, historyFilename));
        }

        private Command GetCommand(string commandName)
        {
            _commandLine.Commands.TryGetValue(commandName, out var command);
            return command;
        }

        private void OnGUI()
        {
            var newText = _inputField.Draw(new Rect(0, Screen.height - _height, Screen.width, _height), _inputStyle, _coloredInputStyle);
            if (_inputField.Text != newText)
                _inputField.SetText(newText, _syntaxHighlighter, GetCommand);
        }

        [Command]
        private static void Hello()
        { }

        [ContextMenu(nameof(Execute))]
        private void Execute()
        {
            _commandLine.Execute(input);
            input = "";
        }
    }
}