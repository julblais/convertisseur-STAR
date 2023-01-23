using System;
using System.Collections.Generic;

namespace STAR.Format
{
    static class RulesHelpers
    {
        public static SplitResult SplitInTwo(this ReadOnlySpan<char> text, ReadOnlySpan<char> delimiter)
        {
            var result = text.IndexOf(delimiter);
            if (result != -1) //found
            {
                var first = text.Slice(0, result);
                var last = text.Slice(result + delimiter.Length);
                return new SplitResult(first, last);
            }

            return SplitResult.CreateEmpty();
        }

        public static SplitResult SplitInTwo(this ReadOnlySpan<char> text, char delimiter)
        {
            var result = text.IndexOf(delimiter);
            if (result != -1) //found
            {
                var first = text.Slice(0, result);
                var last = text.Slice(result + 1);
                return new SplitResult(first, last);
            }

            return SplitResult.CreateEmpty();
        }

        public static ReadOnlySpan<char> StartTrimUntil(this ReadOnlySpan<char> text, ReadOnlySpan<char> delimiter)
        {
            var result = text.IndexOf(delimiter);
            if (result != -1) //found
                return text.Slice(result);
            return text;
        }

        public static ReadOnlySpan<char> StartTrimUntil(this ReadOnlySpan<char> text, char delimiter)
        {
            var result = text.IndexOf(delimiter);
            if (result != -1) //found
                return text.Slice(result);
            return text;
        }

        public static IEnumerable<Command> ReplaceSubString(IEnumerable<Command> input, ReadOnlySpan<char> substring, Command toReplace)
        {
            var newCommands = new List<Command>();

            foreach (var command in input)
            {
                if (command.type == Command.Type.Text)
                {
                    var text = command.textAsSpan;
                    var result = text.SplitInTwo(substring);
                    while (!result.IsEmpty()) //found
                    {
                        newCommands.Add(Command.CreateText(result.first));
                        newCommands.Add(toReplace);
                        text = result.last;
                        result = text.SplitInTwo(substring);
                    }
                    newCommands.Add(Command.CreateText(text));
                }
                else
                    newCommands.Add(command);
            }

            return newCommands;
        }

        public static IEnumerable<Command> ReplaceSubString(IEnumerable<Command> input, ReadOnlySpan<char> substring, ReadOnlySpan<char> value)
        {
            var command = Command.CreateText(value);
            return ReplaceSubString(input, substring, command);
        }

        public static IEnumerable<Command> ReplaceSubString(IEnumerable<Command> input, char substring, ReadOnlySpan<char> value)
        {
            return ReplaceSubString(input, new ReadOnlySpan<char>(substring), value);
        }

        public static IEnumerable<Command> ReplaceSubString(IEnumerable<Command> input, char substring, Command toReplace)
        {
            return ReplaceSubString(input, new ReadOnlySpan<char>(substring), toReplace);
        }
    }
}