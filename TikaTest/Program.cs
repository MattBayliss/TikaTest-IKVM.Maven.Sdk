// See https://aka.ms/new-console-template for more information
using org.apache.tika;
using org.apache.tika.sax;
using org.apache.tika.parser;
using ikvm.io;
using org.apache.tika.metadata;
using org.apache.tika.detect;
using org.apache.tika.parser.microsoft.ooxml;
using org.apache.tika.parser.microsoft;
using org.apache.tika.config;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;
using org.apache.log4j.config;

var filePath = @"C:\Temp\test.docx";

using var fs = new FileStream(filePath, FileMode.Open);
using var stream = new InputStreamWrapper(fs);

Assembly.Load("slf4j.simple");

// via https://github.com/KevM/tikaondotnet/pull/152#issuecomment-1713804124
string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location!)!;
string[] parserAssemblies = Directory.GetFiles(assemblyPath!, "*.parser.*.dll", SearchOption.AllDirectories);

foreach (string file in parserAssemblies)
{
    Assembly.LoadFile(file);
}



BodyContentHandler handler = new BodyContentHandler();
Parser parser = new AutoDetectParser();
Metadata metadata = new Metadata();
ParseContext context = new ParseContext();

parser.parse(stream, handler, metadata, context);

string text = handler.toString();

using (var sw = new StreamWriter(@"C:\Temp\output.txt", false))
{
    sw.WriteLine(text);
}
Console.WriteLine($"{filePath}: {text}");
