using DickinsonBros.Encryption.Certificate.Abstractions;
using DickinsonBros.Encryption.Certificate.Extensions;
using DickinsonBros.Encryption.Certificate.Runner.Models;
using DickinsonBros.Encryption.Certificate.Runner.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
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
                var services = InitializeDependencyInjection();
                ConfigureServices(services);

                using (var provider = services.BuildServiceProvider())
                {
                    var configurationEncryptionService = provider.GetRequiredService<IConfigurationEncryptionService>();
                    var certificateEncryptionService = provider.GetRequiredService<ICertificateEncryptionService<Standard>>();
                    var hostApplicationLifetime = provider.GetService<IHostApplicationLifetime>();

                    hostApplicationLifetime.ApplicationStopped.Register(() =>
                    {
                        Console.WriteLine("ApplicationStopped Start");
                        Task.Delay(TimeSpan.FromDays(1)).Wait();
                        Console.WriteLine("ApplicationStopped End");
                    });

                    {
                        var encryptedString = certificateEncryptionService.Encrypt("Sample123!");
                        var decryptedString = certificateEncryptionService.Decrypt(encryptedString);
                        var encryptedByteArray = certificateEncryptionService.EncryptToByteArray("Sample123!");
                        var decryptedStringFromByteArray = certificateEncryptionService.Decrypt(encryptedByteArray);
                        Console.WriteLine(
$@"CertificateEncryptionService<Develop>
Encrypted String
{ encryptedString }

Decrypted string
{ decryptedString }

Encrypted To ByteArray
{  Encoding.UTF8.GetString(encryptedByteArray) }

Decrypted String
{ decryptedStringFromByteArray }
");
                    }

                    {
                        var encryptedString = configurationEncryptionService.Encrypt("Sample123!");
                        var decryptedString = configurationEncryptionService.Decrypt(encryptedString);
                        var encryptedByteArray = configurationEncryptionService.EncryptToByteArray("Sample123!");
                        var decryptedStringFromByteArray = configurationEncryptionService.Decrypt(encryptedByteArray);
                        Console.WriteLine(
$@"ConfigurationEncryptionService
Encrypted String
{ encryptedString }

Decrypted string
{ decryptedString }

Encrypted To ByteArray
{  Encoding.UTF8.GetString(encryptedByteArray) }

Decrypted String
{ decryptedStringFromByteArray }
");
                    }
                }
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

        private void ConfigureServices(IServiceCollection services)
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
            
            services.AddSingleton<IHostApplicationLifetime, HostApplicationLifetime>();
            services.AddConfigurationEncryptionService();
            services.AddCertificateEncryptionService<Standard>();
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

