using System.Reflection;

namespace Bulka.CommandLine.Commands
{
    public class MethodProcedureCommand : ProcedureCommand
    {
        private static readonly object[] arguments = new object[1];

        private readonly object _targetObject;
        private readonly MethodInfo _methodInfo;

        public MethodProcedureCommand(Bulka.CommandLine.CommandLine commandLine, string[] argumentNames, object targetObject, MethodInfo methodInfo)
            : base(commandLine, argumentNames)
        {
            _targetObject = targetObject;
            _methodInfo   = methodInfo;
        }
        public override void Invoke(ProcedureArguments arguments)
        {
            MethodProcedureCommand.arguments[0] = arguments;
            _methodInfo.Invoke(_targetObject, MethodProcedureCommand.arguments);
        }
    }
}