using System;
using System.Collections.Generic;
using System.Text;

namespace STAR.Format
{
    public class Formatter
    {
        List<Action<FormattingContext>> m_Rules = new List<Action<FormattingContext>>();

        public void AddRule(Action<FormattingContext> rule)
        {
            m_Rules.Add(rule);
        }

        public void Format(FormattingContext str)
        {
            foreach (var rule in m_Rules)
                rule.Invoke(str);

        }

        public string Format(string str)
        {
            var formattingContext = new FormattingContext(str);
            Format(new FormattingContext(str));
            return formattingContext.ToString();
        }
    }

    public static class FormatterBuilder
    {
        public static Formatter With(this Formatter formatter, Action<FormattingContext> rule)
        {
            formatter.AddRule(rule);
            return formatter;
        }
    }
}
