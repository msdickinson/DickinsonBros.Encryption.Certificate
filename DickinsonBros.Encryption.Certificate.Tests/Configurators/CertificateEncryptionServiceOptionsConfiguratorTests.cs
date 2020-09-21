﻿using DickinsonBros.Encryption.Certificate.Configurators;
using DickinsonBros.Encryption.Certificate.Models;
using DickinsonBros.Test;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace DickinsonBros.Redactor.Tests.Configurator
{

    [TestClass]
    public class CertificateEncryptionServiceOptionsConfiguratorTests : BaseTest
    {
        public class TestClass
        {

        }

        [TestMethod]
        public async Task Configure_Runs_DecryptCalled()
        {
            var certificateEncryptionServiceOptionsOfObject = new CertificateEncryptionServiceOptions<TestClass>
            {
                StoreLocation = "SampleStoreLocation",
                ThumbPrint = "SampleThumbPrint"
            };
            var configurationRoot = BuildConfigurationRoot(certificateEncryptionServiceOptionsOfObject);

            await RunDependencyInjectedTestAsync
            (
                async (serviceProvider) =>
                {
                    //Setup

                    //Act
                    var options = serviceProvider.GetRequiredService<IOptions<CertificateEncryptionServiceOptions<TestClass>>>().Value;

                    //Assert
                    Assert.IsNotNull(options);

                    Assert.AreEqual(certificateEncryptionServiceOptionsOfObject.StoreLocation, options.StoreLocation);
                    Assert.AreEqual(certificateEncryptionServiceOptionsOfObject.ThumbPrint, options.ThumbPrint);

                    await Task.CompletedTask.ConfigureAwait(false);

                },
                serviceCollection => ConfigureServices(serviceCollection, configurationRoot)
            );
        }

        #region Helpers

        private IServiceCollection ConfigureServices(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddOptions();
            serviceCollection.AddSingleton<IConfiguration>(configuration);
            serviceCollection.AddSingleton<IConfigureOptions<CertificateEncryptionServiceOptions<TestClass>>, CertificateEncryptionServiceOptionsConfigurator<TestClass>>();

            return serviceCollection;
        }

        #endregion
    }
}
