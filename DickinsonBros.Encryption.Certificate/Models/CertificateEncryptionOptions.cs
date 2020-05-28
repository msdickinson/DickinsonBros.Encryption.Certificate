using System.Diagnostics.CodeAnalysis;

namespace DickinsonBros.Encryption.Certificate.Models
{

    [ExcludeFromCodeCoverage]
    public class CertificateEncryptionOptions
    {
        public string ThumbPrint { get; set; }
        public string StoreLocation { get; set; }
    }
}
