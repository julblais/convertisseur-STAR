using STAR.Format;
using System.Globalization;
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
        int m_ItalicsLevel;

        public WordWriter(string title)
        {
            m_Title = title;
        }

        void BeginDocument(TextWriter writer)
        {
            m_ItalicsLevel = 0;

            string headerFormatted = string.Format(CultureInfo.InvariantCulture, headerFormat, m_Title);
            writer.Write(headerFormatted);
            writer.WriteLine();
        }

        static void EndDocument(TextWriter writer)
        {
            writer.Write(footer);
        }

        public void WriteCommand(in Command command, TextWriter writer)
        {
            switch (command.Type)
            {
                case CommandType.Begin:
                    BeginDocument(writer);
                    break;
                case CommandType.End:
                    EndDocument(writer);
                    break;
                case CommandType.Text:
                    writer.Write(command.Text);
                    break;
                case CommandType.Newline:
                    writer.WriteLine(NewLine);
                    break;
                case CommandType.ItalicsBegin:
                    writer.Write(BeginItalics);
                    m_ItalicsLevel++;
                    break;
                case CommandType.ItalicsEnd:
                    writer.Write(EndItalics);
                    m_ItalicsLevel--;
                    break;
                case CommandType.NewSection:
                    while (m_ItalicsLevel-- > 0)//close italics scope to avoid spilling italics over the next sections
                        writer.WriteLine(EndItalics);
                    writer.WriteLine();
                    writer.WriteLine(NewLine);
                    writer.WriteLine(NewLine);
                    writer.WriteLine();
                    break;
                default:
                    break;
            }
        }
    }
}
