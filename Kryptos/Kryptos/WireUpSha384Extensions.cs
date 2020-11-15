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
            var sha384Command = new Command("sha384", "SHA384");
            var sha384HashCommand = new Command("hash", "Hash");
            sha384HashCommand.AddOption(new Option(new string[] { "--text", "-t" }, "Input Text")
            {
                Argument = new Argument<string>("text")
            });
            sha384HashCommand.AddOption(new Option(new string[] { "--input", "-i" }, "Input file path")
            {
                Argument = new Argument<FileInfo>("input")
            });
            sha384HashCommand.AddOption(new Option(new string[] { "--output", "-o" }, "Output file path")
            {
                Argument = new Argument<FileInfo>("output")
            });
            sha384HashCommand.Handler = CommandHandler.Create<string, FileInfo, FileInfo, IConsole>(async (text, input, output, console) =>
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

                    using var sha384 = SHA384.Create();
                    var hashBytes = sha384.ComputeHash(inputStream);
                    if (output == null)
                    {
                        console.Out.WriteLine(hashBytes.ToHexString());
                    }
                    else
                    {
                        await outputStream.WriteAsync(hashBytes);
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
                        await inputStream.DisposeAsync();
                    }
                    if (outputStream != null)
                    {
                        await outputStream.DisposeAsync();
                    }
                }
            });

            sha384Command.Add(sha384HashCommand);
            rootCommand.AddCommand(sha384Command);

            return rootCommand;
        }
    }
}
