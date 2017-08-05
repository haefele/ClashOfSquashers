using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;

namespace MatchMaker.Shared.Common
{
    public static class Guard
    {
        [DebuggerStepThrough]
        public static void NotNull(object argument, string argumentName)
        {
            if (argument == null)
                throw new ArgumentNullException(argumentName);
        }
        [DebuggerStepThrough]
        public static void NotNullOrWhiteSpace(string argument, string argumentName)
        {
            NotNull(argument, argumentName);

            if (string.IsNullOrWhiteSpace(argument))
                throw new ArgumentException("String is whitespace.", argumentName);
        }

        [DebuggerStepThrough]
        public static void NotNullOrEmpty(IEnumerable argument, string argumentName)
        {
            NotNull(argument, argumentName);

            if (argument.GetEnumerator().MoveNext() == false)
                throw new ArgumentException("List is empty.", argumentName);
        }

        [DebuggerStepThrough]
        public static void NotInvalidEnum(object argument, string argumentName)
        {
            NotNull(argument, argumentName);

            if (argument.GetType().GetTypeInfo().IsEnum == false)
                throw new InvalidOperationException("The NotInvalidEnum only works with enum values.");

            if (Enum.IsDefined(argument.GetType(), argument) == false)
                throw new ArgumentException("Unknown enum value.", argumentName);
        }
    }
}