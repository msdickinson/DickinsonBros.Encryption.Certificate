using DickinsonBros.Encryption.Certificate.Abstractions;
using DickinsonBros.Encryption.Certificate.Configurators;
using DickinsonBros.Encryption.Certificate.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace DickinsonBros.Encryption.Certificate.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCertificateEncryptionService<T>(this IServiceCollection serviceCollection)
        where T : CertificateEncryptionServiceOptionsType
        {
            serviceCollection.TryAddSingleton(typeof(ICertificateEncryptionService<T>), typeof(CertificateEncryptionService<T>));
            serviceCollection.TryAddSingleton(typeof(IConfigureOptions<CertificateEncryptionServiceOptions<T>>), typeof(CertificateEncryptionServiceOptionsConfigurator<T>));
            return serviceCollection;
        }

        public static IServiceCollection AddConfigurationEncryptionService(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton(typeof(IConfigurationEncryptionService), typeof(CertificateEncryptionService<Configuration>));
            serviceCollection.TryAddSingleton(typeof(IConfigureOptions<CertificateEncryptionServiceOptions<Configuration>>), typeof(CertificateEncryptionServiceOptionsConfigurator<Configuration>));
            return serviceCollection;
        }
    }
}
