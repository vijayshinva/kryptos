using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Kryptos
{
    public static class WireUpSha256Extensions
    {
        public static RootCommand WireUpSha256Commands(this RootCommand rootCommand)
        {
            var sha256Command = new Command("sha256", "Secure Hash Algorithm 2 - 256");
            var sha256HashCommand = new Command("hash", "Hash");
            sha256HashCommand.AddOption(new Option<string>(new string[] { "--text", "-t" }, "Input Text"));
            sha256HashCommand.AddOption(new Option<FileInfo>(new string[] { "--input", "-i" }, "Input file path"));
            sha256HashCommand.AddOption(new Option<FileInfo>(new string[] { "--output", "-o" }, "Output file path"));

            sha256HashCommand.Handler = CommandHandler.Create<string, FileInfo, FileInfo, IConsole>(async (text, input, output, console) =>
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

                    using var sha256 = SHA256.Create();
                    var hashBytes = sha256.ComputeHash(inputStream);
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

            sha256Command.Add(sha256HashCommand);
            rootCommand.AddCommand(sha256Command);

            return rootCommand;
        }
    }
}
