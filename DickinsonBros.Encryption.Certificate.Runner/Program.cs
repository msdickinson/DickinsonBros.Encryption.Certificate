using DickinsonBros.Encryption.Certificate.Abstractions;
using DickinsonBros.Encryption.Certificate.Extensions;
using DickinsonBros.Encryption.Certificate.Models;
using DickinsonBros.Encryption.Certificate.Runner.Models;
using DickinsonBros.Encryption.Certificate.Runner.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DickinsonBros.Encryption.Certificate.Runner
{
    class Program
    {
        IConfiguration _configuration;
        async static Task Main()
        {
            await new Program().DoMain();
        }
        async Task DoMain()
        {
            try
            {
                using var applicationLifetime = new ApplicationLifetime();
                var services = InitializeDependencyInjection();
                ConfigureServices(services, applicationLifetime);

                using (var provider = services.BuildServiceProvider())
                {
                    var certificateEncryption = provider.GetRequiredService<ICertificateEncryption<RunnerCertificateEncryptionOptions>>();

                    var encryptedString = certificateEncryption.Encrypt("Sample123!");
                    var decryptedString = certificateEncryption.Decrypt(encryptedString);
                    Console.WriteLine(
                $@"Encrypted String
{ Convert.ToBase64String(encryptedString) }

Decrypted String
{ decryptedString }
");
                }
                applicationLifetime.StopApplication();
                await Task.CompletedTask.ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                Console.WriteLine("End...");
                Console.ReadKey();
            }
        }

        private void ConfigureServices(IServiceCollection services, ApplicationLifetime applicationLifetime)
        {
            services.AddOptions();
            services.AddLogging(config =>
            {
                config.AddConfiguration(_configuration.GetSection("Logging"));

                if (Environment.GetEnvironmentVariable("BUILD_CONFIGURATION") == "DEBUG")
                {
                    config.AddConsole();
                }
            });
            services.AddSingleton<IApplicationLifetime>(applicationLifetime);
            services.AddCertificateEncryption<RunnerCertificateEncryptionOptions>();
            services.Configure<CertificateEncryptionOptions<RunnerCertificateEncryptionOptions>>(_configuration.GetSection(nameof(RunnerCertificateEncryptionOptions)));

        }

        IServiceCollection InitializeDependencyInjection()
        {
            var aspnetCoreEnvironment = Environment.GetEnvironmentVariable("BUILD_CONFIGURATION");
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile($"appsettings.{aspnetCoreEnvironment}.json", true);
            _configuration = builder.Build();
            var services = new ServiceCollection();
            services.AddSingleton(_configuration);
            return services;
        }
    }
}

