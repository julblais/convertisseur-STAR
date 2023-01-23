using System;
using System.Collections;
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

        public static void ReplaceSubString(
            IEnumerable<Command> input, ReadOnlySpan<char> substring, Command toReplace, ICollection<Command> output)
        {
            foreach (var command in input)
            {
                if (command.type == Command.Type.Text)
                {
                    var text = command.textAsSpan;
                    var result = text.SplitInTwo(substring);
                    while (!result.IsEmpty()) //found
                    {
                        output.Add(Command.CreateText(result.first));
                        output.Add(toReplace);
                        text = result.last;
                        result = text.SplitInTwo(substring);
                    }
                    output.Add(Command.CreateText(text));
                }
                else
                    output.Add(command);
            }
        }

        public static void ReplaceSubString(
            IEnumerable<Command> input, ReadOnlySpan<char> substring, ReadOnlySpan<char> value, ICollection<Command> output)
        {
            var command = Command.CreateText(value);
            ReplaceSubString(input, substring, command, output);
        }

        public static void ReplaceSubString(
            IEnumerable<Command> input, char substring, ReadOnlySpan<char> value, ICollection<Command> output)
        {
            ReplaceSubString(input, new ReadOnlySpan<char>(substring), value, output);
        }

        public static void ReplaceSubString(
            IEnumerable<Command> input, char substring, Command toReplace, ICollection<Command> output)
        {
            ReplaceSubString(input, new ReadOnlySpan<char>(substring), toReplace, output);
        }
    }
}