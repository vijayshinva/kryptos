using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.IO;

namespace Kryptos
{
    public static class WireUpUuidExtensions
    {
        public static RootCommand WireUpUuidCommands(this RootCommand rootCommand)
        {
            var guidCommand = new Command("uuid", "Universally Unique Identifier");
            guidCommand.AddOption(new Option<bool>(new string[] { "--no-hyphens", "--no-dash", "-n" }, "No Hyphens"));
            guidCommand.AddOption(new Option<FileInfo>(new string[] { "--output", "-o" }, "Output file path"));

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
                    return 22;
                }
                return 0;
            });

            rootCommand.AddCommand(guidCommand);

            return rootCommand;
        }
    }
}
