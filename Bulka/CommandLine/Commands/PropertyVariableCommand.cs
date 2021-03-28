using System.Reflection;

namespace Bulka.CommandLine.Commands
{
    public class PropertyVariableCommand : VariableCommand
    {
        private readonly object _targetObject;
        private readonly PropertyInfo _propertyInfo;
        public  bool IsPure { get; }

        public PropertyVariableCommand(Bulka.CommandLine.CommandLine commandLine, object targetObject, PropertyInfo propertyInfo, bool save=true, bool isPure = false) 
            : base(commandLine, save)
        {
            _targetObject = targetObject;
            _propertyInfo = propertyInfo;
            IsPure        = isPure;
        }
        public override object GetValue()
        {
            return (object)_propertyInfo.GetValue(_targetObject);
        }
        public override void SetValue(object value)
        {
            _propertyInfo.SetValue(_targetObject, value);
        }
    }
}