using Japanese.Core.Hashing;

namespace khothemegiatot.Hashify.Hashing;

public static class StringExtensions
{
    public static string MD5(this string input) => HashFunctionHelper.ComputeMD5(input);
    public static string SHA256(this string input) => HashFunctionHelper.ComputeSHA256(input);
    public static string SHA384(this string input) => HashFunctionHelper.ComputeSHA384(input);
    public static string SHA512(this string input) => HashFunctionHelper.ComputeSHA512(input);
}