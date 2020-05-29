# DickinsonBros.Encryption.Certificate

<a href="https://www.nuget.org/packages/DickinsonBros.Encryption.Certificate/">
    <img src="https://img.shields.io/nuget/v/DickinsonBros.Encryption.Certificate">
</a>

Encrypt and decrypt strings with certificates

Features
* Certificate based encryption 
* Configure certificate location
* Built with Generics allowing multiple configurations and instances concurrently.

<a href="https://dev.azure.com/marksamdickinson/DickinsonBros/_build?definitionScope=%5CDickinsonBros.Encryption.Certificate">Builds</a>

<h2>Example Usage</h2>

```C#
var encryptedByteArray = certificateEncryptionService.Encrypt("Sample123!");
var encryptedString = Convert.ToBase64String(encryptedByteArray);
var decryptedString = certificateEncryptionService.Decrypt(encryptedByteArray);
Console.WriteLine(
$@"Encrypted String
{ encryptedString }

Decrypted String
{ decryptedString }
");
```
    
    Encrypted String
    f4KFyOSqPAM8ju2q+521D3S2zGNuvsNks382GOlgL8C3VyaWVhCF4VS0bIyoQjK8KsoI7mQ8Uu8w54TkzCHuFGqXOmLJU0Rfjurjn+01VCxBsgo1G23u4QUtM5uXBSye/S/jcXGVLDJX90F7gss+NdKvbhebq6jFnFsR6ZhrTGc7BLbLiE0M/BE7A+8hxCGjOFXvgwBm8nTFhXh/sSV8fbZ9pCzwcPuSXMTKxRi+cji3jN42hJidmOBNKIXi2pq6hIL5kcDxKXuVxznOOOcwh/clfCa8Hx6rY/q1O4y14AT5IknCnvYXWCEroXfvX1vlemXewL/UCN486c6VzGssGA==

    Decrypted String
    Sample123!

Example Runner Included in folder "DickinsonBros.Encryption.Certificate.Runner"

<h2>Setup</h2>

<h3>Install a windows certificate</h3>

Below will show you have to install a cert with the private key and without.
You can only decrypt if you have the cert with the private key.

Note in all 3 powershell scripts you may decide to replace "DemoConfig" and the password "5cce8bf4-0cd7-4c22-9968-8793b9938db1" as these are just examples.

<h4>Create powershell scripts</h3>

<h5>CreateCert.ps1</h5>
    
    $cert = New-SelfSignedCertificate -Type DocumentEncryptionCert -Subject "CN=DemoConfig" -KeyExportPolicy Exportable -KeySpec KeyExchange

    Export-Certificate -Cert $cert -FilePath ".\DemoConfig.cer"

    $mypwd = ConvertTo-SecureString -String "5cce8bf4-0cd7-4c22-9968-8793b9938db1" -Force -AsPlainText

    Export-PfxCertificate -Cert $cert -FilePath ".\DemoConfig.pfx" -Password $mypwd

    $cert

<h5>ImportCert.ps1</h5>

    Import-Certificate -FilePath ".\DemoConfig.cer" -CertStoreLocation Cert:\LocalMachine\My
    
<h5>ImportCertWithPrivateKey.ps1</h5>

    $mypwd = ConvertTo-SecureString -String "5cce8bf4-0cd7-4c22-9968-8793b9938db1" -Force -AsPlainText

    Import-PfxCertificate -FilePath ".\DemoConfig.pfx" -CertStoreLocation Cert:\LocalMachine\My -Password $mypwd
    
<h4>Run CreateCert.ps1</h3>

Running this will generate two files

    //Certificate
    DemoConfig.cer
    
    //Certificate With Private Key
    DemoConfig.pfx

<h4>Run importCert.ps1 OR ImportCertWithPrivateKey.ps1</h3>

This will install the Certificate and give you the ThumbPrint.

To verfiy certificate is installed (and where you can remove it)
* mmc.exe
* File -> Add/Remove Snap In
* Click Certificate and then Add
* Select computer account and press next
* Select local computer and press finsh
* Select ok to close the 
* Select the folder personal/Certificates
* Look for your your cert in the example above it would be "DemoConfig"

<h3>Add Nuget References</h3>

    https://www.nuget.org/packages/DickinsonBros.Encryption.Certificate/
    https://www.nuget.org/packages/DickinsonBros.Encryption.Certificate.Abstractions

<h3>Create class with base of CertificateEncryptionServiceOptions</h3>

```c#
public class RunnerCertificateEncryptionServiceOptions : CertificateEncryptionServiceOptions
{

};
```
<h3>Create Instance</h3>

```c#
var runnerCertificateEncryptionServiceOptions = new RunnerCertificateEncryptionServiceOptions
{
    ThumbPrint = "...",
    StoreLocation = "..."
};
var options = Options.Create(certificateEncryptionOptions);
var certificateEncryptionService = new CertificateEncryptionService<RunnerCertificateEncryptionServiceOptions>(options);

```

<h3>Create Instance (With Dependency Injection)</h3>

<h4>Add appsettings.json File With Contents</h4>

 ```json  
{
  "RunnerCertificateEncryptionServiceOptions": {
    "ThumbPrint": "...",
    "StoreLocation": "..."
  }
}
 ```    
<h4>Code</h4>

```c#

var serviceCollection = new ServiceCollection();

//Configure Options
var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", false)

var configuration = builder.Build();
serviceCollection.AddOptions();
services.Configure<CertificateEncryptionServiceOptions<RunnerCertificateEncryptionServiceOptions>>(_configuration.GetSection(nameof(RunnerCertificateEncryptionServiceOptions)));

//Add Service
services.AddCertificateEncryptionService<RunnerCertificateEncryptionServiceOptions>();

//Build Service Provider 
using (var provider = services.BuildServiceProvider())
{
  var certificateEncryptionService = provider.GetRequiredService<ICertificateEncryptionService<RunnerCertificateEncryptionServiceOptions>>();
}
```
