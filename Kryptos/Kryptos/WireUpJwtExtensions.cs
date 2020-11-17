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
            jwtDecCommand.AddOption(new Option(new string[] { "--text", "-t" }, "Input Text")
            {
                Argument = new Argument<string>("text")
            });
            jwtDecCommand.AddOption(new Option(new string[] { "--input", "-i" }, "Input file path")
            {
                Argument = new Argument<FileInfo>("input")
            });
            jwtDecCommand.AddOption(new Option(new string[] { "--output", "-o" }, "Output file path")
            {
                Argument = new Argument<FileInfo>("output")
            });
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
                        jwt = Jwt.Decode(File.ReadAllText(input.FullName));
                    }

                    if (output == null)
                    {
                        console.Out.WriteLine(jwt);
                    }
                    else
                    {
                        File.WriteAllText(output.FullName, jwt);
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
