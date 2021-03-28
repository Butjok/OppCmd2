using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Bulka.CommandLine.Utilities
{
    public class AssertException : Exception
    {
        public AssertException(string message = null,
            [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string memberName = null)
            : base($"<b>{message}</b>\n{Path.GetFileName(filePath)}:{lineNumber}: {memberName}(): <i>{File.ReadLines(filePath).Skip(lineNumber - 1).Take(1).First().Trim()}</i>")
        { }
    }
}