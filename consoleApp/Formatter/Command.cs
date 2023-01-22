using System;

namespace STAR.Format
{
    public readonly record struct Command
    {
        public enum Type
        {
            Text,
            Newline,
            ItalicsBegin,
            ItalicsEnd,
        }

        public readonly Type type;
        public readonly ReadOnlyMemory<char> text;
        public ReadOnlySpan<char> textAsSpan => text.Span;

        Command(Type type, ReadOnlyMemory<char> text)
        {
            this.type = type;
            this.text = text;
        }

        Command(Type type)
        {
            this.type = type;
            text = string.Empty.AsMemory();
        }

        public override string ToString()
        {
            if (type == Type.Text)
                return $"Text: {text}";
            else if (type == Type.Newline)
                return "Endl";
            else if (type == Type.ItalicsBegin)
                return "<ItalicsBegin>";
            else if (type == Type.ItalicsEnd)
                return "<ItalicsEnd>";
            else return "Invalid type";
        }

        public static Command CreateText(ReadOnlyMemory<char> text)
        {
            return new Command(Type.Text, text);
        }

        public static Command CreateText(string text)
        {
            return new Command(Type.Text, text.AsMemory());
        }

        public static Command CreateNewLine()
        {
            return new Command(Type.Newline);
        }

        public static Command CreateItalicsBegin()
        {
            return new Command(Type.ItalicsBegin);
        }

        public static Command CreateItalicsEnd()
        {
            return new Command(Type.ItalicsEnd);
        }
    }
}
