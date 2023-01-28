using System.Collections.Generic;

namespace STAR.Format
{
    public readonly struct CommandContext
    {
        public readonly string originalContent;
        public readonly IEnumerable<Command> input;
        public readonly ICollection<Command> output;

        public CommandContext(string content, IEnumerable<Command> input)
        {
            originalContent = content;
            this.input = input;
            output = new List<Command>();
        }

        public readonly void Add(Command command)
        {
            output.Add(command);
        }
    }
}
