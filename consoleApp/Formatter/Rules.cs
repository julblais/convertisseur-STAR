using System;
using System.Collections.Generic;

namespace STAR.Format
{
    public static class Rules
    {
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
            ReadOnlySpan<char> longSpace = "                 "; //17 spaces
            return RulesHelpers.ReplaceSubString(input, longSpace, " ");
        }

        public static IEnumerable<Command> FixItalicsStart(IEnumerable<Command> input)
        {
            const char italicStart = '\t';
            var command = Command.CreateItalicsBegin();
            return RulesHelpers.ReplaceSubString(input, italicStart, command);
        }

        public static IEnumerable<Command> FixItalicsEnd(IEnumerable<Command> input)
        {
            const char italicsEnd = '\u000E';
            var command = Command.CreateItalicsEnd();
            return RulesHelpers.ReplaceSubString(input, italicsEnd, command);
        }
    }
}
