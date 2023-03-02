using STAR.Format;
using System.IO;

namespace STAR.Writer
{
    public interface IDocumentWriter
    {
        string extension { get; }
        void WriteCommand(in Command command, TextWriter writer);
    }
}
