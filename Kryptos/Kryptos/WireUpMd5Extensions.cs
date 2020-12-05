using System;
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
            md5HashCommand.AddOption(new Option<string>(new string[] { "--text", "-t" }, "Input Text"));
            md5HashCommand.AddOption(new Option<FileInfo>(new string[] { "--input", "-i" }, "Input file path"));
            md5HashCommand.AddOption(new Option<FileInfo>(new string[] { "--output", "-o" }, "Output file path"));

            md5HashCommand.Handler = CommandHandler.Create<string, FileInfo, FileInfo, IConsole>(async (text, input, output, console) =>
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

                    using var md5 = MD5.Create();
                    var hashBytes = md5.ComputeHash(inputStream);
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

            md5Command.Add(md5HashCommand);
            rootCommand.AddCommand(md5Command);

            return rootCommand;
        }
    }
}
