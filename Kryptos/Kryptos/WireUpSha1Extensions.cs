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
    public static class WireUpSha1Extensions
    {
        public static RootCommand WireUpSha1Commands(this RootCommand rootCommand)
        {
            var sha1Command = new Command("sha1", "Secure Hash Algorithm 1");
            var sha1HashCommand = new Command("hash", "Hash");
            sha1HashCommand.AddOption(new Option<string>(new string[] { "--text", "-t" }, "Input Text"));
            sha1HashCommand.AddOption(new Option<FileInfo>(new string[] { "--input", "-i" }, "Input file path"));
            sha1HashCommand.AddOption(new Option<FileInfo>(new string[] { "--output", "-o" }, "Output file path"));

            sha1HashCommand.Handler = CommandHandler.Create<string, FileInfo, FileInfo, IConsole>(async (text, input, output, console) =>
            {
                Stream outputStream = null;
                Stream inputStream = null;
                try
                {
                    if (output == null)
                    {
                        outputStream = new MemoryStream();
                    }
                    else
                    {
                        outputStream = output.OpenWrite();
                    }

                    if (text != null)
                    {
                        inputStream = new MemoryStream(Encoding.UTF8.GetBytes(text));
                    }
                    if (input != null)
                    {
                        inputStream = input.OpenRead();
                    }
                    using var sha1 = SHA1.Create();
                    var hashBytes = sha1.ComputeHash(inputStream);
                    if (output == null)
                    {
                        console.Out.WriteLine(hashBytes.ToHexString());
                    }
                    else
                    {
                        await outputStream.WriteAsync(hashBytes).ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    console.Out.WriteLine(ex.Message);
                }
                finally
                {
                    if (inputStream != null)
                    {
                        await inputStream.DisposeAsync().ConfigureAwait(false);
                    }
                    if (outputStream != null)
                    {
                        await outputStream.DisposeAsync().ConfigureAwait(false);
                    }
                }
            });

            sha1Command.Add(sha1HashCommand);
            rootCommand.AddCommand(sha1Command);

            return rootCommand;
        }
    }
}
