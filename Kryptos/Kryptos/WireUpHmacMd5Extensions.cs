﻿using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Kryptos
{
    public static class WireUpHmacMd5Extensions
    {
        public static RootCommand WireUpHmacMd5Commands(this RootCommand rootCommand)
        {
            var hmacmd5Command = new Command("hmacmd5", "Hash based Message Authentication Code - MD5");
            var hmacmd5HashCommand = new Command("hash", "Hash");
            hmacmd5HashCommand.AddOption(new Option<string>(new string[] { "--text", "-t" }, "Input Text"));
            hmacmd5HashCommand.AddOption(new Option<FileInfo>(new string[] { "--input", "-i" }, "Input file path"));
            hmacmd5HashCommand.AddOption(new Option<string>(new string[] { "--keytext", "-kt" }, "Key Text"));
            hmacmd5HashCommand.AddOption(new Option<FileInfo>(new string[] { "--keyinput", "-ki" }, "Key file path"));
            hmacmd5HashCommand.AddOption(new Option<FileInfo>(new string[] { "--output", "-o" }, "Output file path"));

            hmacmd5HashCommand.Handler = CommandHandler.Create<string, FileInfo, string, FileInfo, FileInfo, IConsole>(async (text, input, keytext, keyinput, output, console) =>
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

                    using var hmacMd5 = new HMACMD5(key);
                    var hashBytes = hmacMd5.ComputeHash(inputStream);
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

            hmacmd5Command.Add(hmacmd5HashCommand);
            rootCommand.AddCommand(hmacmd5Command);

            return rootCommand;
        }
    }
}
