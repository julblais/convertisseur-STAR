using System;
using System.Collections.Generic;
using System.Globalization;

namespace STAR.ConsoleApp
{
    static class ArgumentParser
    {
        public static string[] Parse<T>(this string[] args, string argument, ref T reference)
        {
            int index = GetArgIndex(args, argument);

            if (index < 0) //no arg
                return args;

            if (index + 1 >= args.Length) //at end: no value
                throw new InvalidOperationException($"No value set for argument {argument}");
            else if (args[index + 1].StartsWith("--", StringComparison.InvariantCultureIgnoreCase)) //no value
                throw new InvalidOperationException($"No value set for argument {argument}");
            else
                reference = ConvertTo<T>(argument, args[index + 1]);

            return args;
        }

        public static string[] Parse(this string[] args, string argument, ref bool reference)
        {
            int index = GetArgIndex(args, argument);

            if (index < 0) //no arg
                return args;

            if (index + 1 >= args.Length) //at end: no value
                reference = true;
            else if (args[index + 1].StartsWith("--", StringComparison.InvariantCultureIgnoreCase)) //no value
                reference = true;
            else
                reference = ConvertTo<bool>(argument, args[index + 1]);

            return args;
        }

        public static string[] ParseArgsList(this string[] args)
        {
            var output = new List<string>(args.Length);

            if (args.Length > 0)
            {
                foreach (var arg in args)
                {
                    var isOption = arg.StartsWith("--", StringComparison.InvariantCultureIgnoreCase);

                    if (!isOption)
                        output.Add(arg);
                    else
                        break;
                }
            }
            return output.ToArray();
        }

        static int GetArgIndex(string[] arguments, string argument)
        {
            for (int i = 0; i < arguments.Length; i++)
                if (arguments[i] == argument)
                    return i;
            return -1;
        }

        static TValue ConvertTo<TValue>(string argument, string argumentValue)
        {
            object converted = Convert.ChangeType(argumentValue, typeof(TValue), CultureInfo.InvariantCulture);
            if (converted == null)
                throw new InvalidOperationException($"Cannot parse argument value {argumentValue} for {argument}");
            return (TValue)converted;
        }
    }
}
