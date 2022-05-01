using System.IO;
using System.Reflection;
using R.Systems.Shared.Core.Interfaces;

namespace R.Systems.Lexica.FunctionalTests.Common.Services;

public class EmbeddedRsaKeys : IRsaKeys
{
    public EmbeddedRsaKeys()
    {
        PublicKey = GetPublicKey();
        PrivateKey = GetPrivateKey();
    }

    public string? PublicKey { get; init; }

    public string? PrivateKey { get; init; }

    private string GetPublicKey()
    {
        ResourceLoader resourceLoader = new();
        Assembly assembly = GetType().Assembly;
        const string publicKeyFileName = "public.pem";
        string publicKey = resourceLoader.GetEmbeddedResourceString(assembly, publicKeyFileName);
        if (publicKey == null)
        {
            throw new FileNotFoundException($"RSA public key file ({publicKeyFileName}) doesn't exist.");
        }
        return publicKey;
    }

    private string GetPrivateKey()
    {
        ResourceLoader resourceLoader = new();
        Assembly assembly = GetType().Assembly;
        const string privateKeyFileName = "private.pem";
        string privateKey = resourceLoader.GetEmbeddedResourceString(assembly, privateKeyFileName);
        if (privateKey == null)
        {
            throw new FileNotFoundException($"RSA private key file ({privateKeyFileName}) doesn't exist.");
        }
        return privateKey;
    }
}
