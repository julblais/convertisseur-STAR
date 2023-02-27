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
                case Command.Type.Text:
                    writer.Write(command.text);
                    break;
                case Command.Type.Newline:
                    writer.WriteLine();
                    break;
                case Command.Type.ItalicsBegin:
                    writer.Write("<i>");
                    break;
                case Command.Type.ItalicsEnd:
                    writer.Write("</i>");
                    break;
                case Command.Type.NewSection:
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
