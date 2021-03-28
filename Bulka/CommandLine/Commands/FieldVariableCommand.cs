using System.Reflection;

namespace Bulka.CommandLine.Commands
{
    public class FieldVariableCommand : VariableCommand
    {
        private readonly object _targetObject;
        private readonly FieldInfo _fieldInfo;

        public FieldVariableCommand(Bulka.CommandLine.CommandLine commandLine, object targetObject, FieldInfo fieldInfo, bool save=true) : base(commandLine, save)
        {
            _targetObject = targetObject;
            _fieldInfo    = fieldInfo;
        }
        public override object GetValue()
        {
            return _fieldInfo.GetValue(_targetObject);
        }
        public override void SetValue(object value)
        {
            _fieldInfo.SetValue(_targetObject, value);
        }

        public override string ToString()
        {
            return $"{Name}: field {_fieldInfo.Name} of {_targetObject}";
        }
    }
}