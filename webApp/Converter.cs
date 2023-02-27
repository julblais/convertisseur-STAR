using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using STAR.Format;
using STAR.Writer;
using System.Text;

namespace webApp
{
    public static class Converter
    {
        public static readonly int codepage = 28591; //ISO-8859-1 Western European

        public static async Task<string> Convert(IBrowserFile file, long maxFileSizeBytes)
        {
            if (file == null) return string.Empty;

            using var inputStream = file.OpenReadStream(maxFileSizeBytes);
            var commands = await GetCommands(inputStream);
            return WriteCommands(commands, file.Name);
        }

        public static async Task CreateFileAndDownload(string text, string fileName, IJSRuntime js)
        {
            if (string.IsNullOrEmpty(text)) return;

            using var outputStream = new MemoryStream(Encoding.UTF8.GetBytes(text));
            using var outputStreamRef = new DotNetStreamReference(outputStream);
            await js.InvokeVoidAsync("downloadFileFromStream", fileName, outputStreamRef);
        }

        static async Task<IEnumerable<Command>> GetCommands(Stream stream)
        {
            var contents = await ReadFile(stream);

            var rules = new Formatter.Rule[]
            {
                Rules.FixEndline,
                Rules.FixStartRecord,
                Rules.AddRecordSections,
                Rules.FixLongSpaces,
                Rules.RemoveItalicsStart,
                Rules.RemoveItalicsEnd
            };

            return rules.ApplyTo(contents);
        }

        static string WriteCommands(IEnumerable<Command> commands, string fileName)
        {
            using var writer = new StringWriter();
            var documentWriter = new WordWriter(fileName);
            commands.WriteTo(documentWriter, writer);
            return writer.ToString();
        }

        static async Task<string> ReadFile(Stream stream)
        {
            var encoding = Encoding.GetEncoding(codepage);
            using var reader = new StreamReader(stream, encoding);
            StringBuilder builder = new StringBuilder();

            var line = await reader.ReadLineAsync();
            builder.AppendLine(line);
            while (line != null)
            {
                line = await reader.ReadLineAsync();
                builder.AppendLine(line);
            }
            return builder.ToString();
        }
    }
}
