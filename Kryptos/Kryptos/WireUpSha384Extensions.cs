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
    public static class WireUpSha384Extensions
    {
        public static RootCommand WireUpSha384Commands(this RootCommand rootCommand)
        {
            var sha384Command = new Command("sha384", "Secure Hash Algorithm 2 - 384");
            var sha384HashCommand = new Command("hash", "Hash");
            sha384HashCommand.AddOption(new Option<string>(new string[] { "--text", "-t" }, "Input Text"));
            sha384HashCommand.AddOption(new Option<FileInfo>(new string[] { "--input", "-i" }, "Input file path"));
            sha384HashCommand.AddOption(new Option<FileInfo>(new string[] { "--output", "-o" }, "Output file path"));

            sha384HashCommand.Handler = CommandHandler.Create<string, FileInfo, FileInfo, IConsole>(async (text, input, output, console) =>
            {
                Stream inputStream = null;
                try
                {
                    if (text != null)
                    {
                        inputStream = new MemoryStream(Encoding.UTF8.GetBytes(text));
                    }
                    if (input != null)
                    {
                        inputStream = input.OpenRead();
                    }

                    using var sha384 = SHA384.Create();
                    var hashBytes = sha384.ComputeHash(inputStream);
                    if (output == null)
                    {
                        console.Out.WriteLine(hashBytes.ToHexString());
                    }
                    else
                    {
                        await File.WriteAllBytesAsync(output.FullName, hashBytes).ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    console.Out.WriteLine(ex.Message);
                    return 22;
                }
                finally
                {
                    if (inputStream != null)
                    {
                        await inputStream.DisposeAsync().ConfigureAwait(false);
                    }
                }
                return 0;
            });

            sha384Command.Add(sha384HashCommand);
            rootCommand.AddCommand(sha384Command);

            return rootCommand;
        }
    }
}
