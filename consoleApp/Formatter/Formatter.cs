using System.Collections.Generic;
using System.IO;

namespace STAR.Format
{
    public static class Formatter
    {
        public delegate void Rule(IEnumerable<Command> input, ICollection<Command> output);
        public delegate void Writer(IEnumerable<Command> commands, TextWriter writer);

        public static IEnumerable<Command> ApplyTo(this IEnumerable<Rule> rules, string str)
        {
            IEnumerable<Command> commands = new Command[]
            {
                Command.CreateText(str)
            };

            List<Command> output = new();

            foreach (var rule in rules)
            {
                rule.Invoke(commands, output);
                commands = output;
                output = new List<Command>();
            }

            return commands;
        }

        public static void WriteTo(this IEnumerable<Command> commands, Writer writer, TextWriter textWriter)
        {
            writer.Invoke(commands, textWriter);
        }
    }
}
