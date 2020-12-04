using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Kryptos
{
    public static class WireUpPfxExtensions
    {
        public static RootCommand WireUpPfxCommands(this RootCommand rootCommand)
        {
            var pfxCommand = new Command("pfx", "PFX, PKCS #12");
            var pfxEncCommand = new Command("encrypt", "Encrypt");
            pfxEncCommand.AddAlias("enc");
            pfxEncCommand.AddOption(new Option<string>(new string[] { "--text", "-t" }, "Input Text"));
            pfxEncCommand.AddOption(new Option<FileInfo>(new string[] { "--input", "-i" }, "Input file path"));
            pfxEncCommand.AddOption(new Option<FileInfo>(new string[] { "--output", "-o" }, "Output file path"));
            pfxEncCommand.AddOption(new Option<FileInfo>(new string[] { "--certinput", "-ci" }, "Certificate file path (.pfx)"));
            pfxEncCommand.AddOption(new Option<string>(new string[] { "--keytext", "-kt" }, "Key Text"));

            pfxEncCommand.Handler = CommandHandler.Create<string, FileInfo, FileInfo, FileInfo, string, IConsole>(async (text, input, output, certInput, keyText, console) =>
            {
                try
                {
                    using var certificate = new X509Certificate2(certInput.FullName, keyText);
                    using var rsa = certificate.GetRSAPublicKey();

                    if (input != null)
                    {
                        text = await File.ReadAllTextAsync(input.FullName).ConfigureAwait(false);
                    }
                    var encryptedText = rsa.Encrypt(Encoding.UTF8.GetBytes(text), RSAEncryptionPadding.Pkcs1);
                    if (output == null)
                    {
                        console.Out.WriteLine(Convert.ToBase64String(encryptedText));
                    }
                    else
                    {
                        var outputStream = output.OpenWrite();
                        await outputStream.WriteAsync(encryptedText).ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    console.Out.WriteLine(ex.Message);
                    return 22;
                }
                return 0;
            });

            var pfxDecCommand = new Command("decrypt", "Decrypt");
            pfxDecCommand.AddAlias("dec");
            pfxDecCommand.AddOption(new Option<string>(new string[] { "--text", "-t" }, "Input Text"));
            pfxDecCommand.AddOption(new Option<FileInfo>(new string[] { "--input", "-i" }, "Input file path"));
            pfxDecCommand.AddOption(new Option<FileInfo>(new string[] { "--output", "-o" }, "Output file path"));
            pfxDecCommand.AddOption(new Option<FileInfo>(new string[] { "--certinput", "-ci" }, "Certificate file path (.pfx)"));
            pfxDecCommand.AddOption(new Option<string>(new string[] { "--keytext", "-kt" }, "Key Text"));

            pfxDecCommand.Handler = CommandHandler.Create<string, FileInfo, FileInfo, FileInfo, string, IConsole>(async (text, input, output, certInput, keyText, console) =>
            {
                try
                {
                    using var certificate = new X509Certificate2(certInput.FullName, keyText, X509KeyStorageFlags.EphemeralKeySet);
                    using var rsa = certificate.GetRSAPrivateKey();

                    var textBytes = Convert.FromBase64String(text);
                    if (input != null)
                    {
                        textBytes = await File.ReadAllBytesAsync(input.FullName).ConfigureAwait(false);
                    }
                    var decryptedText = rsa.Decrypt(textBytes, RSAEncryptionPadding.Pkcs1);
                    if (output == null)
                    {
                        console.Out.WriteLine(Encoding.UTF8.GetString(decryptedText));
                    }
                    else
                    {
                        using var outputStream = output.OpenWrite();
                        await outputStream.WriteAsync(decryptedText).ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    console.Out.WriteLine(ex.Message);
                    return 22;
                }
                return 0;
            });

            var pfxInfoCommand = new Command("info", "Information");
            pfxInfoCommand.AddAlias("info");
            pfxInfoCommand.AddOption(new Option<FileInfo>(new string[] { "--certinput", "-ci" }, "Certificate file path (.pfx)"));
            pfxInfoCommand.AddOption(new Option<string>(new string[] { "--keytext", "-kt" }, "Key Text"));

            pfxInfoCommand.Handler = CommandHandler.Create<FileInfo, string, IConsole>((certInput, keyText, console) =>
            {
                try
                {
                    using var certificate = new X509Certificate2(certInput.FullName, keyText);
                    console.Out.WriteLine($"Thumbprint\t: {certificate.Thumbprint}");
                    console.Out.WriteLine($"Subject\t\t: {certificate.Subject}");
                    console.Out.WriteLine($"FriendlyName\t: {certificate.FriendlyName}");
                    console.Out.WriteLine($"Issuer\t\t: {certificate.Issuer}");
                    console.Out.WriteLine($"NotAfter\t: {certificate.NotAfter:dd-MMM-yyyy}");
                    console.Out.WriteLine($"NotBefore\t: {certificate.NotBefore:dd-MMM-yyyy}");
                    console.Out.WriteLine($"HasPrivateKey\t: {certificate.HasPrivateKey}");
                    console.Out.Write($"Oid\t\t: {certificate.PublicKey.Oid.FriendlyName}");
                }
                catch (Exception ex)
                {
                    console.Out.WriteLine(ex.Message);
                    return 22;
                }
                return 0;
            });

            pfxCommand.Add(pfxEncCommand);
            pfxCommand.Add(pfxDecCommand);
            pfxCommand.Add(pfxInfoCommand);
            rootCommand.AddCommand(pfxCommand);

            return rootCommand;
        }
    }
}
