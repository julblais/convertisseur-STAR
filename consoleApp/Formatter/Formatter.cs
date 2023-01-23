using System.Collections.Generic;
using System.IO;

namespace STAR.Format
{
    public static class Formatter
    {
        public delegate void Rule(FormattingContext context);
        public delegate void Writer(IEnumerable<Command> commands, TextWriter writer);

        public static IEnumerable<Command> ApplyTo(this IEnumerable<Rule> rules, string str)
        {
            var formattingContext = new FormattingContext(str);
            foreach (var rule in rules)
                rule.Invoke(formattingContext);

            return formattingContext.commands;
        }

        public static void WriteTo(this IEnumerable<Command> commands, Writer writer, TextWriter textWriter)
        {
            writer.Invoke(commands, textWriter);
        }
    }
}
