using System.Security.Cryptography;
using System.Text;
using SystemText = System.Text;

namespace OmniSciLab.Hashify.Hashing;

public class HashFunctionHelper
{
    public static string ComputeMD5(string input)
    {
        using MD5 md5 = MD5.Create();

        byte[] bytes = SystemText.Encoding.UTF8.GetBytes(input);
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Chuyển đổi mảng byte sang chuỗi dạng hex
        StringBuilder hash = new StringBuilder();
        foreach (byte b in hashBytes)
        {
            hash.Append(b.ToString("x2"));
        }
        return hash.ToString();
    }

    public static string ComputeSHA256(string input)
    {
        using SHA256 sha256 = SHA256.Create();

        byte[] bytes = SystemText.Encoding.UTF8.GetBytes(input);
        byte[] hashBytes = sha256.ComputeHash(bytes);

        // Chuyển đổi mảng byte sang chuỗi dạng hex
        StringBuilder hash = new StringBuilder();
        foreach (byte b in hashBytes)
        {
            hash.Append(b.ToString("x2"));
        }
        return hash.ToString();
    }

    public static string ComputeSHA384(string input)
    {
        using SHA384 sha384 = SHA384.Create();

        byte[] bytes = SystemText.Encoding.UTF8.GetBytes(input);
        byte[] hashBytes = sha384.ComputeHash(bytes);

        // Chuyển mảng byte sang chuỗi dạng hex
        StringBuilder hash = new StringBuilder();
        foreach (byte b in hashBytes)
        {
            hash.Append(b.ToString("x2"));
        }
        return hash.ToString();
    }

    public static string ComputeSHA512(string input)
    {
        using SHA512 sha512 = SHA512.Create();

        byte[] bytes = SystemText.Encoding.UTF8.GetBytes(input);
        byte[] hashBytes = sha512.ComputeHash(bytes);

        // Chuyển mảng byte sang chuỗi dạng hex
        StringBuilder hash = new StringBuilder();
        foreach (byte b in hashBytes)
        {
            hash.Append(b.ToString("x2"));
        }
        return hash.ToString();
    }
}
