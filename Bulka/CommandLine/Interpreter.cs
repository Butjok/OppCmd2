using System;
using System.Linq;
using Bulka.CommandLine.Commands;
using UnityEngine;

namespace Bulka.CommandLine
{
    [CLSCompliant(false)]
    public class Interpreter : CommandLineBaseListener
    {
        private readonly CommandLine _commandLine;
        private readonly Evaluator _valueVisitor;

        public Interpreter(CommandLine commandLine)
        {
            _commandLine  = commandLine;
            _valueVisitor = new Evaluator();
        }

        public override void EnterCall(CommandLineParser.CallContext context)
        {
            var commandName = context.Word().GetText();
            if (!_commandLine.Commands.TryGetValue(commandName, out var command))
                throw new Exception($"Unknown command '{commandName}'");

            var valueContexts = context.value();

            switch (command)
            {
                case VariableCommand variableCommand:

                    switch (valueContexts.Length)
                    {
                        case 0:
                            Debug.Log(variableCommand.GetValue());
                            break;

                        case 1:
                            variableCommand.SetValue(_valueVisitor.Visit(valueContexts[0]));
                            goto case 0;

                        default:
                            throw new Exception($"Invalid syntax: zero or one value expected only, {valueContexts.Length} given");
                    }

                    break;

                case ProcedureCommand functionCommand:
                    functionCommand.Invoke(new ProcedureArguments(functionCommand, valueContexts.Select(_valueVisitor.Visit).ToArray()));
                    break;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}