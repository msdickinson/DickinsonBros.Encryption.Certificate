using System.Diagnostics.CodeAnalysis;

namespace DickinsonBros.Encryption.Certificate.Models
{

    [ExcludeFromCodeCoverage]
    public abstract class CertificateEncryptionServiceOptions
    {
        public string ThumbPrint { get; set; }
        public string StoreLocation { get; set; }
    }
}
