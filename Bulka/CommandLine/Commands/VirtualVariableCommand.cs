using System;

namespace Bulka.CommandLine.Commands
{
    public class VirtualVariableCommand : VariableCommand
    {
        public Func<object> Getter { get; }
        private readonly Action<object> _setter;
        public  bool IsPure { get; }

        public VirtualVariableCommand(Bulka.CommandLine.CommandLine commandLine, Func<object> getter, Action<object> setter, bool isPure = false, bool save = true) 
            : base(commandLine, save)
        {
            Getter  = getter;
            _setter = setter;
            IsPure  = isPure;
        }
        public override object GetValue()
        {
            return Getter();
        }
        public override void SetValue(object value)
        {
            _setter(value);
        }
    }
}