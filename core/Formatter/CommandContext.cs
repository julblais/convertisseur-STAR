using System.Collections.Generic;

namespace STAR.Format
{
    public readonly struct CommandContext
    {
        public readonly string OriginalContent { get; init; }
        public readonly IEnumerable<Command> Input { get; init; }
        public readonly ICollection<Command> Output { get; init; }

        public CommandContext(string content, IEnumerable<Command> input)
        {
            OriginalContent = content;
            Input = input;
            Output = new List<Command>();
        }

        public readonly void Add(Command command)
        {
            Output.Add(command);
        }
    }
}
