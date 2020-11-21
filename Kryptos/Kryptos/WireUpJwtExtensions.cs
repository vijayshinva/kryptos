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
    public static class WireUpJwtExtensions
    {
        public static RootCommand WireUpJwtCommands(this RootCommand rootCommand)
        {
            var jwtCommand = new Command("jwt", "JSON Web Token");
            
            var jwtDecCommand = new Command("decode", "Decode");
            jwtDecCommand.AddAlias("dec");
            jwtDecCommand.AddOption(new Option<string>(new string[] { "--text", "-t" }, "Input Text"));
            jwtDecCommand.AddOption(new Option<FileInfo>(new string[] { "--input", "-i" }, "Input file path"));
            jwtDecCommand.AddOption(new Option<FileInfo>(new string[] { "--output", "-o" }, "Output file path"));

            jwtDecCommand.Handler = CommandHandler.Create<string, FileInfo, FileInfo, IConsole>(async (text, input, output, console) =>
            {
                try
                {
                    string jwt = null;

                    if (text != null)
                    {
                        jwt = Jwt.Decode(text);
                    }
                    if (input != null)
                    {
                        jwt = Jwt.Decode(await File.ReadAllTextAsync(input.FullName).ConfigureAwait(false));
                    }

                    if (output == null)
                    {
                        console.Out.WriteLine(jwt);
                    }
                    else
                    {
                        await File.WriteAllTextAsync(output.FullName, jwt).ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    console.Out.WriteLine(ex.Message);
                }
            });
            
            jwtCommand.Add(jwtDecCommand);
            rootCommand.AddCommand(jwtCommand);

            return rootCommand;
        }
    }
}
