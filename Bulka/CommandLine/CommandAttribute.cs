using System;

namespace Bulka.CommandLine
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Field, Inherited = false)]
    [CLSCompliant(false)]
    public sealed class CommandAttribute : Attribute
    {
        private static readonly string[] emptyStringList = new string[0];

        public string Name { get; }
        public string Description { get; }
        public string[] Arguments { get; }
        public bool DontSave { get; }
        public bool Pure { get; }

        public CommandAttribute(string[] arguments = null, string description = null, string name = null, bool dontSave = false, bool pure = false)
        {
            Name        = name;
            Arguments   = arguments ?? emptyStringList;
            Description = description;
            DontSave    = dontSave;
            Pure        = pure;
        }
    }
}