using System;
using System.Collections.Generic;

namespace STAR.Format
{
    public static class Rules
    {
        const string longSpace = "                 "; //17 spaces
        const char italicStart = '\t';
        const char italicsEnd = '\u000E';
        const string startGarbage = "\u0013\u0014\u0001\u0012";
        const string recordSection = "Record:  ";

        public static void FixEndline(IEnumerable<Command> input, ICollection<Command> output)
        {
            const char endline = '\u001f';

            var lineSeparator = new ReadOnlySpan<char>(endline);

            foreach (var command in input)
            {
                var contents = command.textAsSpan;
                foreach (var line in contents.EnumerateLines())
                {
                    if (line.EndsWith(lineSeparator)) //can remove and skip to next
                    {
                        var text = line.TrimEnd(lineSeparator);
                        output.Add(Command.CreateText(text));
                        output.Add(Command.CreateNewLine());
                    }
                    else
                    {
                        output.Add(Command.CreateText(line));
                    }
                }
            }
        }

        public static void FixLongSpaces(IEnumerable<Command> input, ICollection<Command> output)
        {
            RulesHelpers.ReplaceSubString(input, longSpace, " ", output);
        }

        public static void FixItalicsStart(IEnumerable<Command> input, ICollection<Command> output)
        {
            var command = Command.CreateItalicsBegin();
            RulesHelpers.ReplaceSubString(input, italicStart, command, output);
        }

        public static void FixItalicsEnd(IEnumerable<Command> input, ICollection<Command> output)
        {
            var command = Command.CreateItalicsEnd();
            RulesHelpers.ReplaceSubString(input, italicsEnd, command, output);
        }

        public static void FixStartRecord(IEnumerable<Command> input, ICollection<Command> output)
        {
            foreach (var command in input)
            {
                if (command.type == Command.Type.Text)
                {
                    if (command.textAsSpan.CompareTo(startGarbage, StringComparison.InvariantCulture) != 0)
                        output.Add(command);
                }
                else
                    output.Add(command);
            }
        }

        public static void AddRecordSections(IEnumerable<Command> input, ICollection<Command> output)
        {
            bool first = true;

            foreach (var command in input)
            {
                if (command.type == Command.Type.Text)
                {
                    if (!first && command.textAsSpan.Contains(recordSection, StringComparison.InvariantCulture))
                    {
                        output.Add(Command.CreateNewSection());
                        output.Add(command);
                    }
                    else
                        output.Add(command);
                    first = false;
                }
                else
                    output.Add(command);
            }
        }
    }
}
