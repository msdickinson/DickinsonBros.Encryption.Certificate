using DickinsonBros.Encryption.Certificate.Abstractions;
using DickinsonBros.Encryption.Certificate.Models;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DickinsonBros.Encryption.Certificate
{
    [ExcludeFromCodeCoverage]
    public class CertificateEncryption<T> : ICertificateEncryption<T>
    {
        internal readonly string _thumbPrint;
        internal readonly StoreLocation _storeLocation;

        public CertificateEncryption(IOptions<CertificateEncryptionOptions<T>> certificateEncryptionOptions)
        {
            _thumbPrint = certificateEncryptionOptions.Value.ThumbPrint;
            _storeLocation = certificateEncryptionOptions.Value.StoreLocation == "LocalMachine"
                                ? StoreLocation.LocalMachine : StoreLocation.CurrentUser;

        }

        public string Decrypt(byte[] encrypted)
        {
            try
            {
                using var x509Store = new X509Store(StoreName.My, _storeLocation);
                x509Store.Open(OpenFlags.ReadOnly);
                var certificateCollection = x509Store.Certificates.Find(X509FindType.FindByThumbprint, _thumbPrint, false);
                if (certificateCollection.Count > 0)
                {
                    var certificate = certificateCollection[0];
                    using var rsaPrivateKey = certificate.GetRSAPrivateKey();
                    return Encoding.ASCII.GetString(rsaPrivateKey.Decrypt(encrypted, RSAEncryptionPadding.Pkcs1));
                }
                else
                {
                    throw new Exception($"No certificate found for Thumbprint {_thumbPrint} in location {_storeLocation}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Unhandled exception. Thumbprint: {_thumbPrint}, Location: {_storeLocation}", ex);
            }
        }

        public byte[] Encrypt(string rawString)
        {
            try
            {
                using var x509Store = new X509Store(StoreName.My, _storeLocation);
                x509Store.Open(OpenFlags.ReadOnly);
                var certificateCollection = x509Store.Certificates.Find(X509FindType.FindByThumbprint, _thumbPrint, false);
                if (certificateCollection.Count > 0)
                {
                    var certificate = certificateCollection[0];
                    using RSA rsa = certificate.GetRSAPrivateKey();
                    byte[] bytestodecrypt = Encoding.UTF8.GetBytes(rawString);
                    return rsa.Encrypt(bytestodecrypt, RSAEncryptionPadding.Pkcs1);
                }
                else
                {
                    throw new Exception($"No certificate found for Thumbprint {_thumbPrint} in location {_storeLocation}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Unhandled exception. Thumbprint: {_thumbPrint}, Location: {_storeLocation}", ex);
            }
        }


    
    }
}
