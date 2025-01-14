using System.Text;

namespace khothemegiatot.WebApi.ExtensionMethods;

public static class RandomExtensions
{
    public static string NextString(this Random random, int length)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var username = new StringBuilder();

        for (int i = 0; i < length; i++)
        {
            username.Append(chars[random.Next(chars.Length)]);
        }

        return username.ToString();
    }
}
