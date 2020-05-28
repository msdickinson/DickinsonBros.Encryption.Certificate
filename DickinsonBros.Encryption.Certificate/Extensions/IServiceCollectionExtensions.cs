using DickinsonBros.Encryption.Certificate.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DickinsonBros.Encryption.Certificate.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCertificateEncryption<T>(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton(typeof(ICertificateEncryption<>), typeof(CertificateEncryption<>));

            return serviceCollection;
        }
    }
}
