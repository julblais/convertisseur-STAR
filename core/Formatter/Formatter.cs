using STAR.Writer;
using System.Collections.Generic;
using System.IO;

namespace STAR.Format
{
    public static class Formatter
    {
        public delegate void Rule(CommandContext context);

        public static IEnumerable<Command> ApplyTo(this IEnumerable<Rule> rules, string str)
        {
            IEnumerable<Command> commands = new Command[]
            {
                Command.CreateText(str)
            };

            CommandContext context = new(str, commands);

            foreach (Rule rule in rules)
            {
                rule.Invoke(context);
                context = new(str, context.Output);
            }

            return context.Input;
        }

        public static void WriteTo(this IEnumerable<Command> commands, IDocumentWriter writer, TextWriter textWriter)
        {
            writer.WriteCommands(commands, textWriter);
        }
    }
}
