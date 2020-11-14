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

            rootCommand.WireUpBase64Commands();

            return await rootCommand.InvokeAsync(args);
        }
    }
}
