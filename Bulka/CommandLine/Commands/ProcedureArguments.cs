using System;

namespace Bulka.CommandLine.Commands
{
    public class ProcedureArguments
    {
        private readonly ProcedureCommand _procedureCommand;
        private readonly object[] _values;
        public int Length => _values.Length;

        public ProcedureArguments(ProcedureCommand command, object[] values)
        {
            _procedureCommand = command;
            _values   = values;
        }

        public object this[int index]
        {
            get
            {
                if (index < 0 || index >= _values.Length)
                    throw new Exception($"Index {index} is out of arguments' range 0..{_values.Length - 1}");
                return _values[index];
            }
        }
        public object this[string argumentName]
        {
            get
            {
                var index = _procedureCommand.GetArgumentIndex(argumentName);
                if (index >= _values.Length)
                    throw new Exception($"Missing a value for argument '{argumentName}'");
                return this[index];
            }
        }

        private T InternalGet<T>(int index)
        {
            var value = this[index];
            try
            {
                return (T) value;
            }
            catch (InvalidCastException)
            {
                var name = _procedureCommand.ArgumentIndexes.TryGetKey(index, out var n)
                    ? $" ('{n}')"
                    : string.Empty;
                throw new Exception($"Cannot cast argument {index}{name} from type {value.GetType()} to {typeof(T)}");
            }
        }

        public T Get<T>(int index)
        {
            return InternalGet<T>(index);
        }
        public T Get<T>(string argumentName)
        {
            var index = _procedureCommand.GetArgumentIndex(argumentName);
            if (index >= _values.Length)
                throw new Exception($"Missing a value for argument '{argumentName}'");
            return InternalGet<T>(index);
        }
    }
}