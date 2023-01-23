using STAR.Format;
using System.Collections.Generic;
using System.Text;

namespace STAR.Writer
{
    public static class MarkDownWriter
    {
        public static string WriteCommands(IEnumerable<Command> commands)
        {
            StringBuilder builder = new StringBuilder();

            foreach (Command command in commands)
            {
                WriteCommand(builder, command);
            }

            return builder.ToString();
        }

        static void WriteCommand(StringBuilder builder, in Command command)
        {
            switch(command.type)
            {
                case Command.Type.Text:
                    builder.Append(command.text);
                    break;
                case Command.Type.Newline:
                    builder.AppendLine();
                    break;
                case Command.Type.ItalicsBegin:
                case Command.Type.ItalicsEnd:
                    builder.Append('*');
                    break;
            }
        }
    }
}
