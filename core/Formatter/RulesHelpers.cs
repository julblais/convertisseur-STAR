using System;

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

        public static void ReplaceSubString(CommandContext ctx, ReadOnlySpan<char> substring, Command toReplace)
        {
            foreach (var command in ctx.input)
            {
                if (command.type == CommandType.Text)
                {
                    var text = command.textAsSpan;
                    var result = text.SplitInTwo(substring);
                    while (!result.IsEmpty()) //found
                    {
                        if (result.first.Length > 0)
                            ctx.Add(Command.CreateText(result.first));
                        ctx.Add(toReplace);
                        text = result.last;
                        result = text.SplitInTwo(substring);
                    }
                    if (text.Length > 0)
                        ctx.Add(Command.CreateText(text));
                }
                else
                    ctx.Add(command);
            }
        }

        public static void ReplaceSubString(CommandContext ctx, ReadOnlySpan<char> substring, ReadOnlySpan<char> value)
        {
            var command = Command.CreateText(value);
            ReplaceSubString(ctx, substring, command);
        }

        public static void ReplaceSubString(CommandContext ctx, char substring, ReadOnlySpan<char> value)
        {
            ReplaceSubString(ctx, new ReadOnlySpan<char>(substring), value);
        }

        public static void ReplaceSubString(CommandContext ctx, char substring, Command toReplace)
        {
            ReplaceSubString(ctx, new ReadOnlySpan<char>(substring), toReplace);
        }

        public static void RemoveSubString(CommandContext ctx, char substring)
        {
            RemoveSubString(ctx, new ReadOnlySpan<char>(substring));
        }

        public static void RemoveSubString(CommandContext ctx, ReadOnlySpan<char> substring)
        {
            foreach (var command in ctx.input)
            {
                if (command.type == CommandType.Text)
                {
                    var text = command.textAsSpan;
                    var result = text.SplitInTwo(substring);
                    while (!result.IsEmpty()) //found
                    {
                        if (result.first.Length > 0)
                            ctx.Add(Command.CreateText(result.first));
                        text = result.last;
                        result = text.SplitInTwo(substring);
                    }
                    if (text.Length > 0)
                        ctx.Add(Command.CreateText(text));
                }
                else
                    ctx.Add(command);
            }
        }
    }
}
