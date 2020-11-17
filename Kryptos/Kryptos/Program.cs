using System;
using System.CommandLine;
using System.Threading.Tasks;

namespace Kryptos
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var rootCommand = new RootCommand("Kryptos")
            {
                TreatUnmatchedTokensAsErrors = true
            };

            rootCommand.WireUpUuidCommands()
                .WireUpBase64Commands()
                .WireUpBase64UrlCommands()
                .WireUpMd5Commands()
                .WireUpSha1Commands()
                .WireUpSha256Commands()
                .WireUpSha384Commands()
                .WireUpSha512Commands()
                .WireUpJwtCommands();

            return await rootCommand.InvokeAsync(args);
        }
    }
}
