using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

// ReSharper disable ExplicitCallerInfoArgument

namespace Bulka.CommandLine.Utilities
{
    public static class AssertExtension
    {
        [Conditional("DEBUG")]
        public static void Assert(this bool condition, Func<string> message = null,
            [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string memberName = null)
        {
            if (condition)
                return;

            throw new AssertException(message?.Invoke(), filePath, lineNumber, memberName);
        }
    }
}