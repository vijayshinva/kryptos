using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.IO;
using System.IO.Compression;
using System.CommandLine.Rendering;

namespace Kryptos
{
    public static class WireUpZipExtensions
    {
        public static RootCommand WireUpZipCommands(this RootCommand rootCommand)
        {
            var zipCommand = new Command("zip", "Zip Archive");

            var infoCommand = new Command("info", "Zip File Information");
            infoCommand.AddOption(new Option<FileInfo>(new string[] { "--input", "-i" }, "Input file path"));
            infoCommand.AddOption(new Option<FileInfo>(new string[] { "--output", "-o" }, "Output file path"));

            infoCommand.Handler = CommandHandler.Create<FileInfo, FileInfo, IConsole>(async (input, output, console) =>
            {
                try
                {
                    using var stream = input.OpenRead();
                    using var zip = new ZipArchive(stream);

                    if (output == null)
                    {
                        console.Append(new ConsoleViewZipInfo(zip));
                    }
                    else
                    {
                        var response = new List<string>(new string[] { "Name,Length,CompressedLength,Crc32" });
                        foreach (var item in zip.Entries)
                        {
                            response.Add($"{item.Name},{item.Length},{item.CompressedLength},{item.Crc32}");
                        }
                        await File.WriteAllLinesAsync(output.FullName, response).ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    console.Out.WriteLine(ex.Message);
                    return 22;
                }
                return 0;
            });

            zipCommand.Add(infoCommand);
            rootCommand.AddCommand(zipCommand);

            return rootCommand;
        }
    }
}
