using STAR.Format;
using System.Collections.Generic;
using System.IO;

namespace STAR.Writer
{
    public class WordWriter : IDocumentWriter
    {
        public string extension => "doc";

        const string headerFormat = @"<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:word' xmlns='http://www.w3.org/TR/REC-html40' lang=""fr"">
<head><title>{0}</title>
<!--[if gte mso 9]>
<xml>
<w:WordDocument>
<w:View>Print</w:View>
<w:DoNotOptimizeForBrowser/>
</w:WordDocument>
</xml>
<![endif]-->
<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"">
<style><!-- 
@page
{{
    size: 21.59cm 27.94cm;
    margin: 2cm 2cm 2cm 2cm;
    mso-page-orientation: portrait;
}}
--></style>
</head>
<body>
<font face=""Times New Roman"">
<div style=""font-size:12pt"">";

        const string footer = @"</div>
</body>
</html>";

        const string pageBreak = "<br clear=all style='mso-special-character:line-break;page-break-before:always'>";
        const string NewLine = "<br>";
        const string BeginItalics = "<i>";
        const string EndItalics = "</i>";

        readonly string m_Title;

        public WordWriter(string title)
        {
            m_Title = title;
        }

        public void WriteCommands(IEnumerable<Command> commands, TextWriter writer)
        {
            WriteHeader(m_Title, writer);
            writer.WriteLine();

            foreach (Command command in commands)
                WriteCommand(writer, command);

            writer.Write(footer);
        }

        static void WriteHeader(string title, TextWriter writer)
        {
            var headerFormatted = string.Format(headerFormat, title);
            writer.Write(headerFormatted);
        }

        static void WriteCommand(TextWriter writer, in Command command)
        {
            switch (command.type)
            {
                case Command.Type.Text:
                    writer.Write(command.text);
                    break;
                case Command.Type.Newline:
                    writer.WriteLine(NewLine);
                    break;
                case Command.Type.ItalicsBegin:
                    writer.Write(BeginItalics);
                    break;
                case Command.Type.ItalicsEnd:
                    writer.Write(EndItalics);
                    break;
                case Command.Type.NewSection:
                    writer.WriteLine();
                    writer.WriteLine(pageBreak);
                    writer.WriteLine();
                    break;
            }
        }
    }
}
