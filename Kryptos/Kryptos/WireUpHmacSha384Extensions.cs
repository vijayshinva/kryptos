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
    public static class WireUpHmacSha384Extensions
    {
        public static RootCommand WireUpHmacSha384Commands(this RootCommand rootCommand)
        {
            var hmacsha384Command = new Command("hmacsha384", "Hash based Message Authentication Code - SHA384");
            var hmacsha384HashCommand = new Command("hash", "Hash");
            hmacsha384HashCommand.AddOption(new Option<string>(new string[] { "--text", "-t" }, "Input Text"));
            hmacsha384HashCommand.AddOption(new Option<FileInfo>(new string[] { "--input", "-i" }, "Input file path"));
            hmacsha384HashCommand.AddOption(new Option<string>(new string[] { "--keytext", "-kt" }, "Key Text"));
            hmacsha384HashCommand.AddOption(new Option<FileInfo>(new string[] { "--keyinput", "-ki" }, "Key file path"));
            hmacsha384HashCommand.AddOption(new Option<FileInfo>(new string[] { "--output", "-o" }, "Output file path"));

            hmacsha384HashCommand.Handler = CommandHandler.Create<string, FileInfo, string, FileInfo, FileInfo, IConsole>(async (text, input, keytext, keyinput, output, console) =>
            {
                Stream outputStream = null;
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

                    byte[] key = null;

                    if (keytext != null)
                    {
                        key = Encoding.UTF8.GetBytes(keytext);
                    }

                    if (keyinput != null)
                    {
                        key = await File.ReadAllBytesAsync(keyinput.FullName).ConfigureAwait(false);
                    }

                    using var hmacSha384 = new HMACSHA384(key);
                    var hashBytes = hmacSha384.ComputeHash(inputStream);
                    if (output == null)
                    {
                        console.Out.WriteLine(hashBytes.ToHexString());
                    }
                    else
                    {
                        outputStream = output.OpenWrite();
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

            hmacsha384Command.Add(hmacsha384HashCommand);
            rootCommand.AddCommand(hmacsha384Command);

            return rootCommand;
        }
    }
}
