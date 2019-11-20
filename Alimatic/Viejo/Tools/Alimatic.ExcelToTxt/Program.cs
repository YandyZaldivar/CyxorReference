using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Alimatic.ExcelToTxt
{
    using ExcelDataReader;

    class Program
    {
        static string ProcessExcel(string filePath)
        {
            var maxLength = 0;
            var rows = new List<List<string>>();

            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            using (var reader = ExcelReaderFactory.CreateReader(stream))
                while (reader.Read())
                {
                    var fields = new List<string>();
                    rows.Add(fields);

                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        var value = reader.GetValue(i).ToString();
                        if (maxLength < value.Length)
                            maxLength = value.Length;
                        fields.Add(value);
                    }
                }

            var sb = new StringBuilder();

            foreach (var row in rows)
            {
                foreach (var field in row)
                    sb.Append(field.PadLeft(maxLength + 1));

                sb.AppendLine();
            }

            return sb.ToString();
        }

        static void Main(string[] args)
        {
            //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var foreColor = Console.ForegroundColor;

            try
            {
                if ((args?.Length ?? 0) != 1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Argument error: provide a valid directory path.");
                    return;
                }

                var directoryPath = args[0];

                foreach (var filePath in Directory.EnumerateFiles(directoryPath))
                {
                    if (Path.GetExtension(filePath) == "din")
                        continue;

                    var content = ProcessExcel(filePath);
                    var path = Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath) + ".din");
                    File.WriteAllText(path, content);
                    Console.WriteLine($"File '{filePath}' processed successfully");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                Console.ForegroundColor = foreColor;
                Console.WriteLine("Press a key to exit...");
                Console.ReadKey(intercept: true);
            }
        }
    }
}
//var filePath = "D:/Documents/Visual Studio 2017/Projects/ExcelNetCore/Book1.xlsx";