using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace Kryptos
{
    public static class WireUpSriExtensions
    {
        public static RootCommand WireUpSriCommands(this RootCommand rootCommand)
        {
            var sriCommand = new Command("sri", "Subresource Integrity");
            var sriHashCommand = new Command("hash", "Hash");
            sriHashCommand.AddOption(new Option<string>(new string[] { "--text", "-t" }, "Input Text"));
            sriHashCommand.AddOption(new Option<FileInfo>(new string[] { "--input", "-i" }, "Input file path"));
            sriHashCommand.AddOption(new Option<Uri>(new string[] { "--uri", "-u" }, "Input Uri"));
            sriHashCommand.AddOption(new Option<FileInfo>(new string[] { "--output", "-o" }, "Output file path"));
            sriHashCommand.AddOption(new Option<int>(new string[] { "--sha" }, "256,384(default),512"));

            sriHashCommand.Handler = CommandHandler.Create<int, string, FileInfo, Uri, FileInfo, IConsole>(async (sha, text, input, uri, output, console) =>
            {
                HashAlgorithm hashAlgorithm = null;
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
                    if (uri != null)
                    {
                        using var client = new HttpClient();
                        inputStream = await client.GetStreamAsync(uri);
                    }

                    hashAlgorithm = sha switch
                    {
                        256 => SHA256.Create(),
                        512 => SHA512.Create(),
                        _ => SHA384.Create(),
                    };

                    var hashBytes = hashAlgorithm.ComputeHash(inputStream);
                    var sri = $"sha{hashAlgorithm.HashSize}-{Convert.ToBase64String(hashBytes)}";
                    if (output == null)
                    {
                        console.Out.WriteLine(sri);
                    }
                    else
                    {
                        await File.WriteAllTextAsync(output.FullName, sri).ConfigureAwait(false);
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
                    if (hashAlgorithm != null)
                    {
                        hashAlgorithm.Dispose();
                    }
                }
            });

            sriCommand.Add(sriHashCommand);
            rootCommand.AddCommand(sriCommand);

            return rootCommand;
        }
    }
}
