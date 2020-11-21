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
    public static class WireUpHmacSha256Extensions
    {
        public static RootCommand WireUpHmacSha256Commands(this RootCommand rootCommand)
        {
            var hmacsha256Command = new Command("hmacsha256", "Hash based Message Authentication Code - SHA256");
            var hmacsha256HashCommand = new Command("hash", "Hash");
            hmacsha256HashCommand.AddOption(new Option<string>(new string[] { "--text", "-t" }, "Input Text"));
            hmacsha256HashCommand.AddOption(new Option<FileInfo>(new string[] { "--input", "-i" }, "Input file path"));
            hmacsha256HashCommand.AddOption(new Option<string>(new string[] { "--keytext", "-kt" }, "Key Text"));
            hmacsha256HashCommand.AddOption(new Option<FileInfo>(new string[] { "--keyinput", "-ki" }, "Key file path"));
            hmacsha256HashCommand.AddOption(new Option<FileInfo>(new string[] { "--output", "-o" }, "Output file path"));

            hmacsha256HashCommand.Handler = CommandHandler.Create<string, FileInfo, string, FileInfo, FileInfo, IConsole>(async (text, input, keytext, keyinput, output, console) =>
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

                    using var hmacSha256 = new HMACSHA256(key);
                    var hashBytes = hmacSha256.ComputeHash(inputStream);
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
                    return 22;
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
                return 0;
            });

            hmacsha256Command.Add(hmacsha256HashCommand);
            rootCommand.AddCommand(hmacsha256Command);

            return rootCommand;
        }
    }
}
