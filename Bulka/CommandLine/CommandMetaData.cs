using System;

namespace Bulka.CommandLine
{
    public class CommandMetaData
    {
        public CommandMetaData(Type declaringType, string description)
        {
            DeclaringType = declaringType;
            Description   = description;
        }
        public Type DeclaringType { get; }
        public string Description { get; }
    }
}