// See https://aka.ms/new-console-template for more information
using org.apache.tika.sax;
using org.apache.tika.parser;
using ikvm.io;
using org.apache.tika.metadata;
using System.Reflection;
using System.Diagnostics;

Assembly.Load("slf4j.simple");

// via https://github.com/KevM/tikaondotnet/pull/152#issuecomment-1713804124
string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location!)!;
string[] parserAssemblies = Directory.GetFiles(assemblyPath!, "*.parser.*.dll", SearchOption.AllDirectories);

foreach (string file in parserAssemblies)
{
    Assembly.LoadFile(file);
}

var inputDir = new DirectoryInfo(@"C:\temp\TikaTests\input");
var outputDir = new DirectoryInfo(@"C:\temp\TikaTests\output");
var errorDir = new DirectoryInfo(@"C:\temp\TikaTests\errors");

if (!inputDir.Exists) inputDir.Create();

if(outputDir.Exists) outputDir.Delete(true);
outputDir.Create();

if(errorDir.Exists) errorDir.Delete(true);
errorDir.Create();

var inputFiles = inputDir.GetFiles();

Stopwatch timer = Stopwatch.StartNew();

Parser parser = new AutoDetectParser();
ParseContext context = new ParseContext();

foreach (var file in inputFiles)
{
    string status = "INIT";
    using var fs = new FileStream(file.FullName, FileMode.Open);
    using var stream = new InputStreamWrapper(fs);

    var handler = new BodyContentHandler();
    Metadata metadata = new Metadata();

    timer.Restart();

    try
    {
        status = "PARSING";
        parser.parse(stream, handler, metadata, context);

        var outputPath = Path.Combine(outputDir.FullName, file.Name) + ".txt";

        using StreamWriter outputFile = new StreamWriter(outputPath, false);
        outputFile.Write(handler.toString());

        status = "PARSED";

    }
    catch (Exception ex)
    {
        status = "ERROR";

        var errorPath = Path.Combine(errorDir.FullName, file.Name) + ".txt";
        using StreamWriter errorFile = new StreamWriter(errorPath, false);
        errorFile.Write(ex.ToString());
    }
    finally
    {
        Console.WriteLine($"{file.Name,22} :: {timer.ElapsedMilliseconds,7} ms :: {status}");
    }
}
Console.WriteLine();
Console.WriteLine("##### DONE #####");
Console.ReadLine();