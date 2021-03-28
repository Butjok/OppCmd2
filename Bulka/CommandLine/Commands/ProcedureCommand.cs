using System;
using System.Text;
using Bulka.CommandLine.Utilities;

namespace Bulka.CommandLine.Commands
{
    public abstract class ProcedureCommand : Command
    {
        private readonly OneToOneMap<string, int> _argumentIndexes;
        public IReadOnlyOneToOneMap<string, int> ArgumentIndexes => _argumentIndexes;

        protected ProcedureCommand(Bulka.CommandLine.CommandLine commandLine, string[] argumentNames) : base(commandLine)
        {
            _argumentIndexes = new OneToOneMap<string, int>();
            for (var i = 0; i < argumentNames.Length; i++)
            {
                var name = argumentNames[i];
                if (_argumentIndexes.ContainsKey(name))
                    throw new Exception($"Duplicate argument '{name}'");
                _argumentIndexes.Add(name, i);
            }
        }
        public int GetArgumentIndex(string name)
        {
            if (!_argumentIndexes.TryGetValue(name, out var index))
                throw new Exception($"Unknown argument '{name}'");
            return index;
        }

        public abstract void Invoke(ProcedureArguments arguments);

        public override string ToString()
        {
            var info = CommandLine.Infos[this];
            var sb = new StringBuilder($"{GetType().Name} {Name} ({string.Join(", ", _argumentIndexes.Keys)})");
            if (info.DeclaringType != null)
                sb.Append($" [from {info.DeclaringType}]");
            if (!string.IsNullOrWhiteSpace(info.Description))
                sb.Append($": {info.Description}");
            return sb.ToString();
        }
    }
}