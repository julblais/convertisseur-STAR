using STAR.Format;
using System.Collections.Generic;
using System.IO;

namespace STAR.Writer
{
    public interface IDocumentWriter
    {
        void WriteCommands(IEnumerable<Command> commands, TextWriter writer);
    }
}
