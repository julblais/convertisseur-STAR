using STAR.Format;
using System.Collections.Generic;
using System.IO;

namespace STAR.Writer
{
    public static class MarkDownWriter
    {
        public const string extension = "md";

        public static void WriteCommands(IEnumerable<Command> commands, TextWriter writer)
        {
            foreach (Command command in commands)
                WriteCommand(writer, command);
        }

        static void WriteCommand(TextWriter writer, in Command command)
        {
            switch(command.type)
            {
                case Command.Type.Text:
                    writer.Write(command.text);
                    break;
                case Command.Type.Newline:
                    writer.WriteLine();
                    writer.WriteLine();
                    break;
                case Command.Type.ItalicsBegin:
                case Command.Type.ItalicsEnd:
                    writer.Write('*');
                    break;
                case Command.Type.NewSection:
                    writer.WriteLine();
                    writer.WriteLine("___");
                    writer.WriteLine();
                    break;
            }
        }
    }
}
