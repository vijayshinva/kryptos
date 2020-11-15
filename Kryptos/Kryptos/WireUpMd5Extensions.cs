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
    public static class WireUpMd5Extensions
    {
        public static RootCommand WireUpMd5Commands(this RootCommand rootCommand)
        {
            var md5Command = new Command("md5", "Message Digest 5");
            var md5HashCommand = new Command("hash", "Hash");
            md5HashCommand.AddOption(new Option(new string[] { "--text", "-t" }, "Input Text")
            {
                Argument = new Argument<string>("text")
            });
            md5HashCommand.AddOption(new Option(new string[] { "--input", "-i" }, "Input file path")
            {
                Argument = new Argument<FileInfo>("input")
            });
            md5HashCommand.AddOption(new Option(new string[] { "--output", "-o" }, "Output file path")
            {
                Argument = new Argument<FileInfo>("output")
            });
            md5HashCommand.Handler = CommandHandler.Create<string, FileInfo, FileInfo, IConsole>(async (text, input, output, console) =>
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

                    using var md5 = MD5.Create();
                    var hashBytes = md5.ComputeHash(inputStream);
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

            md5Command.Add(md5HashCommand);
            rootCommand.AddCommand(md5Command);

            return rootCommand;
        }
    }
}
