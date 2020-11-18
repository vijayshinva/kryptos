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
    public static class WireUpBase64UrlExtensions
    {
        public static RootCommand WireUpBase64UrlCommands(this RootCommand rootCommand)
        {
            var base64UrlCommand = new Command("base64url", "Base64 URL");
            var base64UrlEncCommand = new Command("encode", "Encode");
            base64UrlEncCommand.AddAlias("enc");
            base64UrlEncCommand.AddOption(new Option(new string[] { "--text", "-t" }, "Input Text")
            {
                Argument = new Argument<string>("text")
            });
            base64UrlEncCommand.AddOption(new Option(new string[] { "--input", "-i" }, "Input file path")
            {
                Argument = new Argument<FileInfo>("input")
            });
            base64UrlEncCommand.AddOption(new Option(new string[] { "--output", "-o" }, "Output file path")
            {
                Argument = new Argument<FileInfo>("output")
            });
            base64UrlEncCommand.Handler = CommandHandler.Create<string, FileInfo, FileInfo, IConsole>(async (text, input, output, console) =>
            {
                try
                {
                    string base64UrlEncodedText = null;

                    if (text != null)
                    {
                        base64UrlEncodedText = Base64Url.Encode(text);
                    }
                    if (input != null)
                    {
                        base64UrlEncodedText = Base64Url.Encode(File.ReadAllText(input.FullName));
                    }

                    if (output == null)
                    {
                        console.Out.WriteLine(base64UrlEncodedText);
                    }
                    else
                    {
                        await File.WriteAllTextAsync(output.FullName, base64UrlEncodedText).ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    console.Out.WriteLine(ex.Message);
                }
            });
            var base64UrlDecCommand = new Command("decode", "Decode");
            base64UrlDecCommand.AddAlias("dec");
            base64UrlDecCommand.AddOption(new Option(new string[] { "--text", "-t" }, "Input Text")
            {
                Argument = new Argument<string>("text")
            });
            base64UrlDecCommand.AddOption(new Option(new string[] { "--input", "-i" }, "Input file path")
            {
                Argument = new Argument<FileInfo>("input")
            });
            base64UrlDecCommand.AddOption(new Option(new string[] { "--output", "-o" }, "Output file path")
            {
                Argument = new Argument<FileInfo>("output")
            });
            base64UrlDecCommand.Handler = CommandHandler.Create<string, FileInfo, FileInfo, IConsole>(async (text, input, output, console) =>
            {
                try
                {
                    string base64UrlDecodedText = null;

                    if (text != null)
                    {
                        base64UrlDecodedText = Base64Url.Decode(text);
                    }
                    if (input != null)
                    {
                        base64UrlDecodedText = Base64Url.Decode(File.ReadAllText(input.FullName));
                    }

                    if (output == null)
                    {
                        console.Out.WriteLine(base64UrlDecodedText);
                    }
                    else
                    {
                        await File.WriteAllTextAsync(output.FullName, base64UrlDecodedText).ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    console.Out.WriteLine(ex.Message);
                }
            });
            base64UrlCommand.Add(base64UrlEncCommand);
            base64UrlCommand.Add(base64UrlDecCommand);
            rootCommand.AddCommand(base64UrlCommand);

            return rootCommand;
        }
    }
}
