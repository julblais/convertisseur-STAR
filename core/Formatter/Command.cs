using System;

namespace STAR.Format
{
    public enum CommandType
    {
        Text,
        Newline,
        ItalicsBegin,
        ItalicsEnd,
        NewSection,
    }

    public readonly struct Command
    {
        public readonly CommandType Type { get; init; }
        public readonly ReadOnlyMemory<char> Text { get; init; }
        public ReadOnlySpan<char> TextAsSpan => Text.Span;

        public Command(ReadOnlyMemory<char> text)
        {
            Type = CommandType.Text;
            Text = text;
        }

        public Command(CommandType type)
        {
            Type = type;
            Text = ReadOnlyMemory<char>.Empty;
        }

        public override string ToString()
        {
            if (Type == CommandType.Text)
                return $"Text: {Text}";
            else if (Type == CommandType.Newline)
                return "Endl";
            else if (Type == CommandType.ItalicsBegin)
                return "<ItalicsBegin>";
            else if (Type == CommandType.ItalicsEnd)
                return "<ItalicsEnd>";
            else
                return "Invalid type";
        }

        public static Command CreateText(ReadOnlyMemory<char> text)
        {
            return new Command(text);
        }

        public static Command CreateText(ReadOnlySpan<char> text)
        {
            return CreateText(text.ToString());
        }

        public static Command CreateText(string text)
        {
            return new Command(text.AsMemory());
        }

        public static Command CreateEmptyText()
        {
            return new Command(ReadOnlyMemory<char>.Empty);
        }

        public static Command CreateNewLine()
        {
            return new Command(CommandType.Newline);
        }

        public static Command CreateNewSection()
        {
            return new Command(CommandType.NewSection);
        }

        public static Command CreateItalicsBegin()
        {
            return new Command(CommandType.ItalicsBegin);
        }

        public static Command CreateItalicsEnd()
        {
            return new Command(CommandType.ItalicsEnd);
        }
    }
}
