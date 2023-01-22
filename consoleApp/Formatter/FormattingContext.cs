using System.Collections.Generic;
using System.Text;

namespace STAR.Format
{
    public class FormattingContext
    {
        public IReadOnlyCollection<string> contents { get; set; }

        public FormattingContext(IReadOnlyCollection<string> contents)
        {
            this.contents = contents;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach(var str in contents)
                sb.AppendLine(str);
            return sb.ToString();
        }
    }
}
