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
    public static class WireUpRngExtensions
    {
        public static RootCommand WireUpRngCommands(this RootCommand rootCommand)
        {
            var rngCommand = new Command("rng", "Random Number Generator");
            rngCommand.AddOption(new Option<int>(new string[] { "--max", "-m" }, "Maximum (exclusive)"));
            rngCommand.AddOption(new Option<FileInfo>(new string[] { "--output", "-o" }, "Output file path"));

            rngCommand.Handler = CommandHandler.Create<int, FileInfo, IConsole>(async (max, output, console) =>
            {
                try
                {
                    if (max <= 0)
                    {
                        max = int.MaxValue;
                    }
                    var random = RandomNumberGenerator.GetInt32(max);

                    if (output == null)
                    {
                        console.Out.WriteLine(random.ToString());
                    }
                    else
                    {
                        await File.WriteAllTextAsync(output.FullName, random.ToString()).ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    console.Out.WriteLine(ex.Message);
                    return 22;
                }
                return 0;
            });

            rootCommand.AddCommand(rngCommand);

            return rootCommand;
        }
    }
}
