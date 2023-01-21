using System.Text;

namespace STAR.Format
{
    public class FormattingContext
    {
        public readonly StringBuilder contents;

        public FormattingContext(StringBuilder contents)
        {
            this.contents = contents;
        }
    }
}
