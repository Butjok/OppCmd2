using System;

namespace Bulka.CommandLine.Commands
{
    public class ActionProcedureCommand : ProcedureCommand
    {
        private readonly Action<ProcedureArguments> _action;

        public ActionProcedureCommand(Bulka.CommandLine.CommandLine commandLine, string[] argumentNames, Action<ProcedureArguments> action) : base(commandLine, argumentNames)
        {
            _action = action;
        }

        public override void Invoke(ProcedureArguments arguments)
        {
            _action(arguments);
        }
    }
}