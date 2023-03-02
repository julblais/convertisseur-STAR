using System.IO;
using STAR.Format;

namespace STAR.Writer
{
    public class TxtWriter : IDocumentWriter
    {
        public string extension => "txt";

        public void WriteCommand(in Command command, TextWriter writer)
        {
            switch (command.Type)
            {
                case CommandType.Text:
                    writer.Write(command.Text);
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
