namespace Bulka.CommandLine.Commands
{
    public abstract class VariableCommand : Command
    {
        protected VariableCommand(Bulka.CommandLine.CommandLine commandLine, bool save) : base(commandLine)
        {
            Save = save;
        }

        public abstract object GetValue();
        public abstract void SetValue(object value);
        public bool Save { get; }
    }
}