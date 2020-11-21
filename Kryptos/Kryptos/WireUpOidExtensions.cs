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
    public static class WireUpOidExtensions
    {
        public static RootCommand WireUpOidCommands(this RootCommand rootCommand)
        {
            Command oidCommand = new Command("oid", "Cryptographic object identifier");
            oidCommand.AddOption(new Option<string>(new string[] { "--text", "-t" }, "Input Text"));

            oidCommand.Handler = CommandHandler.Create<string, IConsole>((text, console) =>
            {
                try
                {
                    var oid = new Oid(text);

                    console.Out.WriteLine($"{oid.FriendlyName} : {oid.Value}");
                }
                catch (Exception ex)
                {
                    console.Out.WriteLine(ex.Message);
                    return 22;
                }
                return 0;
            });

            rootCommand.AddCommand(oidCommand);

            return rootCommand;
        }
    }
}
