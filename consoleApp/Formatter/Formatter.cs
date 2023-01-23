using System.Collections.Generic;
using System.IO;

namespace STAR.Format
{
    public static class Formatter
    {
        public delegate IEnumerable<Command> Rule(IEnumerable<Command> input);
        public delegate void Writer(IEnumerable<Command> commands, TextWriter writer);

        public static IEnumerable<Command> ApplyTo(this IEnumerable<Rule> rules, string str)
        {
            IEnumerable<Command> commands = new Command[]
            {
                Command.CreateText(str)
            };

            foreach (var rule in rules)
                commands = rule.Invoke(commands);

            return commands;
        }

        public static void WriteTo(this IEnumerable<Command> commands, Writer writer, TextWriter textWriter)
        {
            writer.Invoke(commands, textWriter);
        }
    }
}
