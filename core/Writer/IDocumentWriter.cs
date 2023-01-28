using STAR.Format;
using System.Collections.Generic;
using System.IO;

namespace STAR.Writer
{
    public interface IDocumentWriter
    {
        string extension { get; }
        void WriteCommands(IEnumerable<Command> commands, TextWriter writer);
    }
}
