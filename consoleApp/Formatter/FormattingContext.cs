using System;
using System.Collections.Generic;

namespace STAR.Format
{
    public class FormattingContext
    {
        public ReadOnlySpan<char> originalContent => m_OriginalContents.AsSpan();
        public IReadOnlyList<Command> commands => m_Commands;

        readonly string m_OriginalContents;
        IReadOnlyList<Command> m_Commands;

        public FormattingContext(string content)
        {
            m_OriginalContents = content;
            m_Commands = new List<Command> { Command.CreateText(content) };
        }

        public IEnumerator<Command> GetEnumerator()
        {
            return commands.GetEnumerator();
        }

        public void SetCommands(IReadOnlyList<Command> commands)
        {
            m_Commands = commands;
        }
    }
}
