using System;
using System.Collections.Generic;
using System.Text;

namespace STAR.Format
{
    public static class Rules
    {
        public static void FixEndline(FormattingContext context)
        {
            const char endline = '\u001f';

            var output = new List<string>(context.contents.Count);

            StringBuilder buffer = new StringBuilder();
            foreach(var content in context.contents)
            {
                buffer.Append(content);
                if (content.EndsWith(endline)) //can remove and skip to next
                {
                    buffer.Remove(buffer.Length -1, 1);
                    //buffer.AppendLine();
                    output.Add(buffer.ToString());
                    buffer = new StringBuilder();
                }
            }

            context.contents = output;
        }
    }
}
