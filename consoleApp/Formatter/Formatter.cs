using System.Text;

namespace STAR.Format
{
    public class Formatter
    {
        List<Action<StringBuilder>> m_Rules = new List<Action<StringBuilder>>();

        public void AddRule(Action<StringBuilder> rule)
        {
            m_Rules.Add(rule);
        }

        public void Format(StringBuilder str)
        {
            foreach (var rule in m_Rules)
                rule.Invoke(str);

        }

        public string Format(string str)
        {
            var stringBuilder = new StringBuilder(str);
            Format(stringBuilder);
            return stringBuilder.ToString();
        }
    }

    public static class FormatterBuilder
    {
        public static Formatter With(this Formatter formatter, Action<StringBuilder> rule)
        {
            formatter.AddRule(rule);
            return formatter;
        }
    }
}
