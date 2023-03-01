using System;

namespace STAR.Format
{
    static class RulesHelpers
    {
        public static SplitResult SplitInTwo(this ReadOnlySpan<char> text, ReadOnlySpan<char> delimiter)
        {
            int result = text.IndexOf(delimiter);
            if (result != -1) //found
            {
                ReadOnlySpan<char> first = text.Slice(0, result);
                ReadOnlySpan<char> last = text.Slice(result + delimiter.Length);
                return new SplitResult(first, last);
            }

            return SplitResult.CreateEmpty();
        }

        public static SplitResult SplitInTwo(this ReadOnlySpan<char> text, char delimiter)
        {
            int result = text.IndexOf(delimiter);
            if (result != -1) //found
            {
                ReadOnlySpan<char> first = text.Slice(0, result);
                ReadOnlySpan<char> last = text.Slice(result + 1);
                return new SplitResult(first, last);
            }

            return SplitResult.CreateEmpty();
        }

        public static ReadOnlySpan<char> StartTrimUntil(this in ReadOnlySpan<char> text, in ReadOnlySpan<char> delimiter)
        {
            int result = text.IndexOf(delimiter);
            if (result != -1) //found
                return text.Slice(result);
            return text;
        }

        public static ReadOnlySpan<char> StartTrimUntil(this in ReadOnlySpan<char> text, char delimiter)
        {
            int result = text.IndexOf(delimiter);
            if (result != -1) //found
                return text.Slice(result);
            return text;
        }

        public static void ReplaceSubString(in CommandContext ctx, in ReadOnlySpan<char> substring, in Command toReplace)
        {
            foreach (Command command in ctx.Input)
            {
                if (command.Type == CommandType.Text)
                {
                    ReadOnlySpan<char> text = command.TextAsSpan;
                    SplitResult result = text.SplitInTwo(substring);
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

        public static void ReplaceSubString(in CommandContext ctx, in ReadOnlySpan<char> substring, in ReadOnlySpan<char> value)
        {
            var command = Command.CreateText(value);
            ReplaceSubString(ctx, substring, command);
        }

        public static void ReplaceSubString(in CommandContext ctx, char substring, in ReadOnlySpan<char> value)
        {
            ReplaceSubString(ctx, new ReadOnlySpan<char>(substring), value);
        }

        public static void ReplaceSubString(in CommandContext ctx, char substring, in Command toReplace)
        {
            ReplaceSubString(ctx, new ReadOnlySpan<char>(substring), toReplace);
        }

        public static void RemoveSubString(in CommandContext ctx, char substring)
        {
            RemoveSubString(ctx, new ReadOnlySpan<char>(substring));
        }

        public static void RemoveSubString(in CommandContext ctx, in ReadOnlySpan<char> substring)
        {
            foreach (Command command in ctx.Input)
            {
                if (command.Type == CommandType.Text)
                {
                    ReadOnlySpan<char> text = command.TextAsSpan;
                    SplitResult result = text.SplitInTwo(substring);
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
