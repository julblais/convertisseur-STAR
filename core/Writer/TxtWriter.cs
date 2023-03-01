using System.Collections.Generic;
using System.IO;
using STAR.Format;

namespace STAR.Writer
{
    public class TxtWriter : IDocumentWriter
    {
        public string extension => "txt";

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
                    break;
                case CommandType.ItalicsBegin:
                    writer.Write("<i>");
                    break;
                case CommandType.ItalicsEnd:
                    writer.Write("</i>");
                    break;
                case CommandType.NewSection:
                    writer.WriteLine();
                    writer.WriteLine("-----------------------");
                    writer.WriteLine();
                    break;
                default:
                    break;
            }
        }
    }
}
