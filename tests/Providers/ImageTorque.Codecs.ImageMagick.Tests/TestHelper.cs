using System.Security.Cryptography;
using System.Text;

namespace ImageTorque.Codecs.ImageMagick.Tests;

public static class TestHelper
{
    public static string CreateHash(ReadOnlySpan<byte> data)
    {
        byte[] hashBytes = SHA256.HashData(data);
        var sb = new StringBuilder();
        for (int i = 0; i < hashBytes.Length; i++)
        {
            sb.Append(hashBytes[i].ToString("x2"));
        }
        return sb.ToString();
    }
}
