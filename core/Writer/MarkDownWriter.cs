using STAR.Format;
using System.Collections.Generic;
using System.IO;

namespace STAR.Writer
{
    public class MarkDownWriter : IDocumentWriter
    {
        public string extension => "md";

        public void WriteCommands(IEnumerable<Command> commands, TextWriter writer)
        {
            foreach (Command command in commands)
                WriteCommand(writer, command);
        }

        static void WriteCommand(TextWriter writer, in Command command)
        {
            switch (command.type)
            {
                case CommandType.Text:
                    writer.Write(command.text);
                    break;
                case CommandType.Newline:
                    writer.WriteLine();
                    writer.WriteLine();
                    break;
                case CommandType.ItalicsBegin:
                case CommandType.ItalicsEnd:
                    writer.Write('*');
                    break;
                case CommandType.NewSection:
                    writer.WriteLine();
                    writer.WriteLine("___");
                    writer.WriteLine();
                    break;
                default:
                    break;
            }
        }
    }
}
