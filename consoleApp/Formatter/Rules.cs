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

        public static IEnumerable<Command> FixEndline(IEnumerable<Command> input)
        {
            const char endline = '\u001f';

            var newCommands = new List<Command>();
            var lineSeparator = new ReadOnlySpan<char>(endline);

            foreach (var command in input)
            {
                var contents = command.textAsSpan;
                foreach (var line in contents.EnumerateLines())
                {
                    if (line.EndsWith(lineSeparator)) //can remove and skip to next
                    {
                        var text = line.TrimEnd(lineSeparator);
                        newCommands.Add(Command.CreateText(text));
                        newCommands.Add(Command.CreateNewLine());
                    }
                    else
                    {
                        newCommands.Add(Command.CreateText(line));
                    }
                }
            }

            return newCommands;
        }


        public static IEnumerable<Command> FixLongSpaces(IEnumerable<Command> input)
        {
            return RulesHelpers.ReplaceSubString(input, longSpace, " ");
        }

        public static IEnumerable<Command> FixItalicsStart(IEnumerable<Command> input)
        {
            var command = Command.CreateItalicsBegin();
            return RulesHelpers.ReplaceSubString(input, italicStart, command);
        }

        public static IEnumerable<Command> FixItalicsEnd(IEnumerable<Command> input)
        {
            var command = Command.CreateItalicsEnd();
            return RulesHelpers.ReplaceSubString(input, italicsEnd, command);
        }

        public static IEnumerable<Command> FixStartRecord(IEnumerable<Command> input)
        {
            var newCommands = new List<Command>();

            foreach (var command in input)
            {
                if (command.type == Command.Type.Text)
                {
                    if (command.textAsSpan.CompareTo(startGarbage, StringComparison.InvariantCulture) != 0)
                        newCommands.Add(command);
                }
                else
                    newCommands.Add(command);
            }

            return newCommands;
        }

        public static IEnumerable<Command> AddRecordSections(IEnumerable<Command> input)
        {
            var newCommands = new List<Command>();
            bool first = true;

            foreach (var command in input)
            {
                if (command.type == Command.Type.Text)
                {
                    if (!first && command.textAsSpan.Contains(recordSection, StringComparison.InvariantCulture))
                    {
                        newCommands.Add(Command.CreateNewSection());
                        newCommands.Add(command);
                    }
                    else
                        newCommands.Add(command);
                    first = false;
                }
                else
                    newCommands.Add(command);
            }

            return newCommands;
        }
    }
}
