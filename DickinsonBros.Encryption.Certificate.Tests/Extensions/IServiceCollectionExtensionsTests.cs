using DickinsonBros.Encryption.Certificate.Extensions;
using DickinsonBros.Encryption.Certificate.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DickinsonBros.Encryption.Certificate.Tests.Extensions
{
    [TestClass]
    public class IServiceCollectionExtensionsTests
    {
        public class SampleCertificateEncryptionOptions  { }

        [TestMethod]
        public void AddEncryptionService_Should_Succeed()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            // Act
            serviceCollection.AddCertificateEncryption<SampleCertificateEncryptionOptions>();

            // Assert

            Assert.IsTrue(serviceCollection.Any(serviceDefinition => serviceDefinition.ServiceType == typeof(ICertificateEncryption<>) &&
                                           serviceDefinition.ImplementationType == typeof(CertificateEncryption<>) &&
                                           serviceDefinition.Lifetime == ServiceLifetime.Singleton));
        }
    }
}
