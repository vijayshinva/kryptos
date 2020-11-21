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
    public static class WireUpSha512Extensions
    {
        public static RootCommand WireUpSha512Commands(this RootCommand rootCommand)
        {
            var sha512Command = new Command("sha512", "Secure Hash Algorithm 2 - 512");
            var sha512HashCommand = new Command("hash", "Hash");
            sha512HashCommand.AddOption(new Option<string>(new string[] { "--text", "-t" }, "Input Text"));
            sha512HashCommand.AddOption(new Option<FileInfo>(new string[] { "--input", "-i" }, "Input file path"));
            sha512HashCommand.AddOption(new Option<FileInfo>(new string[] { "--output", "-o" }, "Output file path"));

            sha512HashCommand.Handler = CommandHandler.Create<string, FileInfo, FileInfo, IConsole>(async (text, input, output, console) =>
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

                    using var sha512 = SHA512.Create();
                    var hashBytes = sha512.ComputeHash(inputStream);
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

            sha512Command.Add(sha512HashCommand);
            rootCommand.AddCommand(sha512Command);

            return rootCommand;
        }
    }
}
