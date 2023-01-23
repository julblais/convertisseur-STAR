using System;
using System.Collections.Generic;
using System.Text;

namespace STAR.Format
{
    public static class Rules
    {
        public static void FixEndline(FormattingContext context)
        {
            const char endline = '\u001f';

            var newCommands = new List<Command>();
            StringBuilder builder = new();
            var lineSeparator = new ReadOnlySpan<char>(endline);

            foreach(var command in context.commands)
            {
                var contents = command.textAsSpan;
                foreach(var line in contents.EnumerateLines())
                {
                    builder.Append(line);
                    if (line.EndsWith(lineSeparator)) //can remove and skip to next
                    {
                        var text = builder.ToString().AsMemory();
                        text = text.TrimEnd(lineSeparator);
                        newCommands.Add(Command.CreateText(text));
                        newCommands.Add(Command.CreateNewLine());
                        builder.Clear();
                    }
                }
            }

            context.SetCommands(newCommands);
        }
    }
}
