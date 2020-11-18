using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Kryptos
{
    public static class WireUpUuidExtensions
    {
        public static RootCommand WireUpUuidCommands(this RootCommand rootCommand)
        {
            var guidCommand = new Command("uuid", "Universally Unique Identifier");
            guidCommand.AddOption(new Option(new string[] { "--no-hyphens", "--no-dash", "-n" }, "No Hyphens")
            {
                Argument = new Argument<bool>("noHyphens")
            });
            guidCommand.AddOption(new Option(new string[] { "--output", "-o" }, "Output file path")
            {
                Argument = new Argument<FileInfo>("output")
            });
            guidCommand.Handler = CommandHandler.Create<bool, FileInfo, IConsole>(async (noHyphens, output, console) =>
            {
                try
                {
                    string uuid;
                    if (noHyphens)
                    {
                        uuid = Guid.NewGuid().ToString("N");
                    }
                    else
                    {
                        uuid = Guid.NewGuid().ToString();
                    }

                    if (output == null)
                    {
                        console.Out.WriteLine(uuid);
                    }
                    else
                    {
                        await File.WriteAllTextAsync(output.FullName, uuid).ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    console.Out.WriteLine(ex.Message);
                }
            });

            rootCommand.AddCommand(guidCommand);

            return rootCommand;
        }
    }
}
