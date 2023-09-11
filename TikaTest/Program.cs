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

var filePath = @"C:\Temp\test.docx";

using var fs = new FileStream(filePath, FileMode.Open);
using var stream = new InputStreamWrapper(fs);

Parser parser1 = new OOXMLParser();
Parser parser2 = new OfficeParser();

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
