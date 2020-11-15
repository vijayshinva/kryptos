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
    public static class WireUpBase64Extensions
    {
        public static RootCommand WireUpBase64Commands(this RootCommand rootCommand)
        {
            var base64Command = new Command("base64", "Base64");
            var base64EncCommand = new Command("encode", "Encode");
            base64EncCommand.AddAlias("enc");
            base64EncCommand.AddOption(new Option(new string[] { "--text", "-t" }, "Input Text")
            {
                Argument = new Argument<string>("text")
            });
            base64EncCommand.AddOption(new Option(new string[] { "--input", "-i" }, "Input file path")
            {
                Argument = new Argument<FileInfo>("input")
            });
            base64EncCommand.AddOption(new Option(new string[] { "--output", "-o" }, "Output file path")
            {
                Argument = new Argument<FileInfo>("output")
            });
            base64EncCommand.Handler = CommandHandler.Create<string, FileInfo, FileInfo, IConsole>(async (text, input, output, console) =>
            {
                try
                {
                    Stream outputStream = null;
                    if (output == null)
                    {
                        outputStream = new MemoryStream();
                    }
                    else
                    {
                        outputStream = output.OpenWrite();
                    }
                    Stream inputStream = null;
                    if (text != null)
                    {
                        inputStream = new MemoryStream(Encoding.UTF8.GetBytes(text));
                    }
                    if (input != null)
                    {
                        inputStream = input.OpenRead();
                    }

                    using (var cryptoStream = new CryptoStream(outputStream, new ToBase64Transform(), CryptoStreamMode.Write))
                    {
                        await inputStream.CopyToAsync(cryptoStream);
                    }
                    await inputStream.DisposeAsync();
                    await outputStream.DisposeAsync();
                    if (output == null)
                    {
                        console.Out.WriteLine(Encoding.UTF8.GetString(((MemoryStream)outputStream).ToArray()));
                    }
                }
                catch (Exception ex)
                {
                    console.Out.WriteLine(ex.Message);
                }
            });
            var base64DecCommand = new Command("decode", "Decode");
            base64DecCommand.AddAlias("dec");
            base64DecCommand.AddOption(new Option(new string[] { "--text", "-t" }, "Input Text")
            {
                Argument = new Argument<string>("text")
            });
            base64DecCommand.AddOption(new Option(new string[] { "--input", "-i" }, "Input file path")
            {
                Argument = new Argument<FileInfo>("input")
            });
            base64DecCommand.AddOption(new Option(new string[] { "--output", "-o" }, "Output file path")
            {
                Argument = new Argument<FileInfo>("output")
            });
            base64DecCommand.Handler = CommandHandler.Create<string, FileInfo, FileInfo, IConsole>(async (text, input, output, console) =>
            {
                try
                {
                    Stream outputStream = null;
                    if (output == null)
                    {
                        outputStream = new MemoryStream();
                    }
                    else
                    {
                        outputStream = output.OpenWrite();
                    }
                    Stream inputStream = null;
                    if (text != null)
                    {
                        inputStream = new MemoryStream(Encoding.UTF8.GetBytes(text));
                    }
                    if (input != null)
                    {
                        inputStream = input.OpenRead();
                    }

                    using (var cryptoStream = new CryptoStream(outputStream, new FromBase64Transform(), CryptoStreamMode.Write))
                    {
                        await inputStream.CopyToAsync(cryptoStream);
                    }
                    await inputStream.DisposeAsync();
                    await outputStream.DisposeAsync();
                    if (output == null)
                    {
                        console.Out.WriteLine(Encoding.UTF8.GetString(((MemoryStream)outputStream).ToArray()));
                    }
                }
                catch (Exception ex)
                {
                    console.Out.WriteLine(ex.Message);
                }
            });
            base64Command.Add(base64EncCommand);
            base64Command.Add(base64DecCommand);
            rootCommand.AddCommand(base64Command);

            return rootCommand;
        }
    }
}
