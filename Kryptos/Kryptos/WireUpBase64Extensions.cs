using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
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
            base64EncCommand.AddArgument(new Argument("text"));
            base64EncCommand.Handler = CommandHandler.Create<string, IConsole>((text, console) =>
            {
                try
                {
                    console.Out.WriteLine(Convert.ToBase64String(Encoding.UTF8.GetBytes(text)));
                }
                catch (Exception ex)
                {
                    console.Out.WriteLine(ex.Message);
                }
            });
            var base64DecCommand = new Command("decode", "Decode");
            base64DecCommand.AddAlias("dec");
            base64DecCommand.AddArgument(new Argument("text"));
            base64DecCommand.Handler = CommandHandler.Create<string, IConsole>((text, console) =>
            {
                try
                {
                    console.Out.WriteLine(Encoding.UTF8.GetString(Convert.FromBase64String(text)));
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
