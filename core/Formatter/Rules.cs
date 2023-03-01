using System;

namespace STAR.Format
{
    public static class Rules
    {
        const string longSpace = "                 "; //17 spaces
        const char italicStart = '\t';
        const char italicsEnd = '\u000E';
        const string startGarbage = "\u0013\u0014\u0001\u0012";
        const string recordSection = "Record:  ";

        public static void FixEndline(CommandContext context)
        {
            const char endline = '\u001f';

            var lineSeparator = new ReadOnlySpan<char>(endline);

            foreach (var command in context.Input)
            {
                var contents = command.TextAsSpan;
                foreach (var line in contents.EnumerateLines())
                {
                    if (line.EndsWith(lineSeparator)) //can remove and skip to next
                    {
                        var text = line.TrimEnd(lineSeparator);
                        context.Add(Command.CreateText(text));
                        context.Add(Command.CreateNewLine());
                    }
                    else
                    {
                        context.Add(Command.CreateText(line));
                    }
                }
            }
        }

        public static void FixLongSpaces(CommandContext context)
        {
            RulesHelpers.ReplaceSubString(context, longSpace, " ");
        }

        public static void FixItalicsStart(CommandContext context)
        {
            var command = Command.CreateItalicsBegin();
            RulesHelpers.ReplaceSubString(context, italicStart, command);
        }

        public static void FixItalicsEnd(CommandContext context)
        {
            var command = Command.CreateItalicsEnd();
            RulesHelpers.ReplaceSubString(context, italicsEnd, command);
        }

        public static void RemoveItalicsStart(CommandContext context)
        {
            RulesHelpers.RemoveSubString(context, italicStart);
        }

        public static void RemoveItalicsEnd(CommandContext context)
        {
            RulesHelpers.RemoveSubString(context, italicsEnd);
        }

        public static void FixStartRecord(CommandContext context)
        {
            foreach (Command command in context.Input)
            {
                if (command.Type == CommandType.Text)
                {
                    if (command.TextAsSpan.CompareTo(startGarbage, StringComparison.InvariantCulture) != 0)
                        context.Add(command);
                }
                else
                    context.Add(command);
            }
        }

        public static void AddRecordSections(CommandContext context)
        {
            bool first = true;

            foreach (Command command in context.Input)
            {
                if (command.Type == CommandType.Text)
                {
                    if (!first && command.TextAsSpan.Contains(recordSection, StringComparison.InvariantCulture))
                    {
                        context.Add(Command.CreateNewSection());
                        context.Add(command);
                    }
                    else
                        context.Add(command);
                    first = false;
                }
                else
                    context.Add(command);
            }
        }
    }
}
