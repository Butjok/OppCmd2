using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Bulka.CommandLine.Commands;
using Bulka.CommandLine.Utilities;

namespace Bulka.CommandLine
{
    public class CommandLine : IAntlrErrorListener<int>, IAntlrErrorListener<IToken>
    {
        private readonly OneToOneMap<string, Command> _commands;
        public IReadOnlyOneToOneMap<string, Command> Commands => _commands;
        private readonly Interpreter _listener;
        private readonly CommandLineLexer _lexer;
        private readonly CommandLineParser _parser;
        private readonly Dictionary<Command, CommandMetaData> _infos = new Dictionary<Command, CommandMetaData>();
        public bool Debug { get; set; } = false;

        public IReadOnlyDictionary<Command, CommandMetaData> Infos => _infos;

        public CommandLine(IEqualityComparer<string> nameComparer = null)
        {
            _commands = new OneToOneMap<string, Command>(nameComparer);
            _listener = new Interpreter(this);

            _lexer  = new CommandLineLexer(null);
            _parser = new CommandLineParser(null);

            _lexer.RemoveErrorListeners();
            _parser.RemoveErrorListeners();

            _lexer.AddErrorListener(this);
            _parser.AddErrorListener(this);
        }

        public void AddCommand(string name, Command command, CommandMetaData metaData=null)
        {
            if (_commands.TryGetValue(name, out var otherCommand))
                throw new Exception($"Cannot add command '{name}', it already exists: {otherCommand}");
            _commands.Add(name, command);
            _infos.Add(command, metaData ?? new CommandMetaData(null, null));

            if (Debug)
                UnityEngine.Debug.Log($"Added command: {command}");
        }

        public void Remove(string name)
        {
            _commands.RemoveByKey(name);
        }
        public void Remove(Command command)
        {
            _commands.RemoveByValue(command);
        }

        public void RegisterStaticCommands()
        {
            RegisterStaticCommands(Assembly.GetExecutingAssembly());
        }
        public void RegisterStaticCommands(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                foreach (var methodInfo in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    var attribute = methodInfo.GetCustomAttribute<CommandAttribute>();
                    if (attribute == null) continue;

                    var name = attribute.Name ?? methodInfo.MakeCommandNameFromMemberInfo();
                    AddCommand(
                        name, 
                        new MethodProcedureCommand(this, attribute.Arguments, null, methodInfo), 
                        new CommandMetaData(type,attribute.Description));
                }
                foreach (var fieldInfo in type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    var attribute = fieldInfo.GetCustomAttribute<CommandAttribute>();
                    if (attribute == null) continue;

                    var name = attribute.Name ?? fieldInfo.MakeCommandNameFromMemberInfo();
                    AddCommand(
                        name,
                        new FieldVariableCommand(this, null, fieldInfo), 
                        new CommandMetaData(type, attribute.Description));
                }
            }
        }

        public void Execute(string input)
        {
            _lexer.SetInputStream(new AntlrInputStream(input));
            _parser.TokenStream = new CommonTokenStream(_lexer);

            ParseTreeWalker.Default.Walk(_listener, _parser.input());
        }

        public string AsText
        {
            get
            {
                var sb = new StringBuilder();
                foreach (var (name, command) in _commands)
                {
                    if (!(command is VariableCommand variable) || !variable.Save)
                        continue;

                    if (Debug && variable is VirtualVariableCommand {IsPure: false})
                    {
                        UnityEngine.Debug.LogWarning($"Variable '{name}' is not marked as pure, skipping");
                        continue;
                    }

                    var value = variable.GetValue();
                    if (value == null)
                        throw new Exception(name);

                    sb.AppendLine($"{name} {value.Format()};");
                }
                return sb.ToString();
            }
        }

        public void Load(string input)
        {
            var old = AsText;
            try
            {
                Execute(input);
            }
            catch
            {
                Execute(old);
                throw;
            }
        }

        public void SyntaxError(TextWriter output, IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            throw new Exception($"{line}:{charPositionInLine}: {msg}");
        }
        public void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            throw new Exception($"{line}:{charPositionInLine}: {msg}");
        }
    }
}