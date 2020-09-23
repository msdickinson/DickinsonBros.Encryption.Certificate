using DickinsonBros.Encryption.Certificate.Abstractions;
using DickinsonBros.Encryption.Certificate.Configurators;
using DickinsonBros.Encryption.Certificate.Extensions;
using DickinsonBros.Encryption.Certificate.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DickinsonBros.Encryption.Certificate.Tests.Extensions
{
    [TestClass]
    public class IServiceCollectionExtensionsTests
    {
        public class Sample { };

        [TestMethod]
        public void AddCertificateEncryptionService_Should_Succeed()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            // Act
            serviceCollection.AddCertificateEncryptionService<Sample>();

            // Assert

            Assert.IsTrue(serviceCollection.Any(serviceDefinition => serviceDefinition.ServiceType == typeof(ICertificateEncryptionService<Sample>) &&
                                           serviceDefinition.ImplementationType == typeof(CertificateEncryptionService<Sample>) &&
                                           serviceDefinition.Lifetime == ServiceLifetime.Singleton));

            Assert.IsTrue(serviceCollection.Any(serviceDefinition => serviceDefinition.ServiceType == typeof(IConfigureOptions<CertificateEncryptionServiceOptions<Sample>>) &&
                               serviceDefinition.ImplementationType == typeof(CertificateEncryptionServiceOptionsConfigurator<Sample>) &&
                               serviceDefinition.Lifetime == ServiceLifetime.Singleton));
        }

        [TestMethod]
        public void AddConfigurationEncryptionService_Should_Succeed()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            // Act
            serviceCollection.AddConfigurationEncryptionService();

            // Assert

            Assert.IsTrue(serviceCollection.Any(serviceDefinition => serviceDefinition.ServiceType == typeof(IConfigurationEncryptionService) &&
                                           serviceDefinition.ImplementationType == typeof(CertificateEncryptionService<Configuration>) &&
                                           serviceDefinition.Lifetime == ServiceLifetime.Singleton));

            Assert.IsTrue(serviceCollection.Any(serviceDefinition => serviceDefinition.ServiceType == typeof(IConfigureOptions<CertificateEncryptionServiceOptions<Configuration>>) &&
                               serviceDefinition.ImplementationType == typeof(CertificateEncryptionServiceOptionsConfigurator<Configuration>) &&
                               serviceDefinition.Lifetime == ServiceLifetime.Singleton));
        }


    }
}
