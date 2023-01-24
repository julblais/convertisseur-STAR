using System.Collections.Generic;
using System.IO;

namespace STAR.Format
{
    public static class Formatter
    {
        public delegate void Rule(CommandContext context);
        public delegate void Writer(IEnumerable<Command> commands, TextWriter writer);

        public static IEnumerable<Command> ApplyTo(this IEnumerable<Rule> rules, string str)
        {
            IEnumerable<Command> commands = new Command[]
            {
                Command.CreateText(str)
            };

            CommandContext context = new(str, commands);

            foreach (var rule in rules)
            {
                rule.Invoke(context);
                context = new(str, context.output);
            }

            return context.input;
        }

        public static void WriteTo(this IEnumerable<Command> commands, Writer writer, TextWriter textWriter)
        {
            writer.Invoke(commands, textWriter);
        }
    }
}
