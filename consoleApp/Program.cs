using STAR.Format;
using System.Text;

namespace STAR.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var formatter = new Formatter()
                .With(Rules.RemoveItalics);
        }
    }
}