using System.Text;

namespace Bulka.CommandLine.Commands
{
    public abstract class Command
    {
        public Bulka.CommandLine.CommandLine CommandLine { get; }

        protected Command(Bulka.CommandLine.CommandLine commandLine)
        {
            CommandLine = commandLine;
        }

        // ReSharper disable once MemberCanBeProtected.Global
        public string Name => CommandLine.Commands.GetKey(this);

        public override string ToString()
        {
            var info = CommandLine.Infos[this];
            var sb = new StringBuilder($"{GetType().Name} {Name}");
            if (info.DeclaringType != null)
                sb.Append($" [from {info.DeclaringType}]");
            if (!string.IsNullOrWhiteSpace(info.Description))
                sb.Append($": {info.Description}");
            return sb.ToString();
        }
    }
}