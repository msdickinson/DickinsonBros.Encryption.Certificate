using DickinsonBros.Encryption.Certificate.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DickinsonBros.Encryption.Certificate.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCertificateEncryptionService<T>(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton(typeof(ICertificateEncryptionService<>), typeof(CertificateEncryptionService<>));

            return serviceCollection;
        }
    }
}
