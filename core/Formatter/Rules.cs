using System;

namespace STAR.Format
{
    public static class Rules
    {
        const string LongSpace = "                 "; //17 spaces
        const char ItalicStart = '\t';
        const char ItalicsEnd = '\u000E';
        const string StartGarbage = "\u0013\u0014\u0001\u0012";
        const string RecordSection = "Record:  ";

        public static void FixEndline(in CommandContext context)
        {
            const char endline = '\u001f';

            var lineSeparator = new ReadOnlySpan<char>(endline);

            foreach (Command command in context.Input)
            {
                ReadOnlySpan<char> contents = command.TextAsSpan;
                foreach (var line in contents.EnumerateLines())
                {
                    if (line.EndsWith(lineSeparator)) //can remove and skip to next
                    {
                        ReadOnlySpan<char> text = line.TrimEnd(lineSeparator);
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

        public static void FixLongSpaces(in CommandContext context)
        {
            RulesHelpers.ReplaceSubString(context, LongSpace, " ");
        }

        public static void FixItalicsStart(in CommandContext context)
        {
            var command = Command.CreateItalicsBegin();
            RulesHelpers.ReplaceSubString(context, ItalicStart, command);
        }

        public static void FixItalicsEnd(in CommandContext context)
        {
            var command = Command.CreateItalicsEnd();
            RulesHelpers.ReplaceSubString(context, ItalicsEnd, command);
        }

        public static void RemoveItalicsStart(in CommandContext context)
        {
            RulesHelpers.RemoveSubString(context, ItalicStart);
        }

        public static void RemoveItalicsEnd(in CommandContext context)
        {
            RulesHelpers.RemoveSubString(context, ItalicsEnd);
        }

        public static void FixStartRecord(in CommandContext context)
        {
            foreach (Command command in context.Input)
            {
                if (command.Type == CommandType.Text)
                {
                    if (command.TextAsSpan.CompareTo(StartGarbage, StringComparison.InvariantCulture) != 0)
                        context.Add(command);
                }
                else
                    context.Add(command);
            }
        }

        public static void AddRecordSections(in CommandContext context)
        {
            bool first = true;

            foreach (Command command in context.Input)
            {
                if (command.Type == CommandType.Text)
                {
                    if (!first && command.TextAsSpan.Contains(RecordSection, StringComparison.InvariantCulture))
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
