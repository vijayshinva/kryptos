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
            guidCommand.AddOption(new Option(new string[] { "--no-hypens", "-nh" }, "No Hypens")
            {
                Argument = new Argument<bool>("noHypens")
            });
            guidCommand.AddOption(new Option(new string[] { "--output", "-o" }, "Output file path")
            {
                Argument = new Argument<FileInfo>("output")
            });
            guidCommand.Handler = CommandHandler.Create<bool, FileInfo, IConsole>(async (noHypens, output, console) =>
            {
                try
                {
                    string uuid;
                    if (noHypens)
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
                        await File.WriteAllTextAsync(output.FullName, uuid);
                    }
                }
                catch (Exception ex)
                {
                    console.Out.WriteLine(ex.Message);
                }
                finally
                {
                }
            });

            rootCommand.AddCommand(guidCommand);

            return rootCommand;
        }
    }
}
