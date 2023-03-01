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

    public readonly record struct Command
    {

        public readonly CommandType type;
        public readonly ReadOnlyMemory<char> text;
        public ReadOnlySpan<char> textAsSpan => text.Span;

        Command(CommandType type, ReadOnlyMemory<char> text)
        {
            this.type = type;
            this.text = text;
        }

        Command(CommandType type)
        {
            this.type = type;
            text = string.Empty.AsMemory();
        }

        public override string ToString()
        {
            if (type == CommandType.Text)
                return $"Text: {text}";
            else if (type == CommandType.Newline)
                return "Endl";
            else if (type == CommandType.ItalicsBegin)
                return "<ItalicsBegin>";
            else if (type == CommandType.ItalicsEnd)
                return "<ItalicsEnd>";
            else return "Invalid type";
        }

        public static Command CreateText(ReadOnlyMemory<char> text)
        {
            return new Command(CommandType.Text, text);
        }

        public static Command CreateText(ReadOnlySpan<char> text)
        {
            return CreateText(text.ToString());
        }

        public static Command CreateText(string text)
        {
            return new Command(CommandType.Text, text.AsMemory());
        }

        public static Command CreateEmptyText()
        {
            return new Command(CommandType.Text, ReadOnlyMemory<char>.Empty);
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
