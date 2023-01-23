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

            var newCommands = new List<Command>();

            foreach(var command in input)
            {
                if (command.type == Command.Type.Text)
                {
                    var startSlice = 0;
                    var text = command.textAsSpan;
                    var result = text.IndexOf(longSpace);
                    while (result != -1) //found
                    {
                        var part = text.Slice(startSlice, result);
                        newCommands.Add(Command.CreateText(part));
                        newCommands.Add(Command.CreateText(""));
                        text = text.Slice(result + longSpace.Length);
                        result = text.IndexOf(longSpace);
                    }
                    newCommands.Add(Command.CreateText(text));
                }
                else
                {
                    newCommands.Add(command);
                }
            }

            return newCommands;
        }
    }
}
