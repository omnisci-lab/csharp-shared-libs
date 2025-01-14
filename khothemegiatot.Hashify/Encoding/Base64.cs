namespace Japanese.Core.Encoding;

public class Base64
{
    public string? Encode(string? input)
    {
        if (input is null)
            return null;

        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input);
        return Convert.ToBase64String(bytes);
    }

    public string? Decode(string? base64)
    {
        if (base64 is null)
            return null;

        byte[] bytes = Convert.FromBase64String(base64);
        return System.Text.Encoding.UTF8.GetString(bytes);
    }

    public bool IsBase64String(string? base64)
    {
        if (base64 is null)
            return false;

        Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
        return Convert.TryFromBase64String(base64, buffer, out int bytesParsed);
    }
}
