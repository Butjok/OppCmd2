using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bulka.CommandLine
{
    public static class CommandLineExtensions
    {
        private static readonly List<string> stringList = new List<string>();

        public static string MakeCommandNameFromMemberInfo(this MemberInfo memberInfo, Func<Type, string> getTypeName, string typeSeparator = ".", string nameSeparator = ".")
        {
            stringList.Clear();
            for (var type = memberInfo.DeclaringType; type != null; type = type.DeclaringType)
                stringList.Add(getTypeName(type));
            stringList.Reverse();
            return string.Join(typeSeparator, stringList) + nameSeparator + memberInfo.Name;
        }
        public static string MakeCommandNameFromMemberInfo(this MemberInfo memberInfo, string typeSeparator = ".", string nameSeparator = ".")
        {
            return MakeCommandNameFromMemberInfo(memberInfo, type => type.Name, typeSeparator, nameSeparator);
        }
    }
}