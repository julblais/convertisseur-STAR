using STAR.Format;
using System.IO;

namespace STAR.Writer
{
    public class MarkDownWriter : IDocumentWriter
    {
        public string extension => "md";

        public void WriteCommand(in Command command, TextWriter writer)
        {
            switch (command.Type)
            {
                case CommandType.Text:
                    writer.Write(command.Text);
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
