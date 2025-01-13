namespace Japanese.Core.Encoding;

public class Base32
{
    public string? Encode(string? input)
    {
        if (input is null)
            return null;

        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input);
        return ToBase32String(bytes);
    }

    public string? Decode(string? base64)
    {
        if (base64 is null)
            return null;

        byte[] bytes = FromBase32String(base64);
        return System.Text.Encoding.UTF8.GetString(bytes);
    }

    private byte[] FromBase32String(string input)
    {
        if (string.IsNullOrEmpty(input))
            throw new ArgumentNullException("input");

        input = input.TrimEnd('=');
        int byteCount = input.Length * 5 / 8;
        byte[] returnArray = new byte[byteCount];

        byte curByte = 0, bitsRemaining = 8;
        int mask = 0, arrayIndex = 0;

        foreach (char c in input)
        {
            int cValue = CharToValue(c);

            if (bitsRemaining > 5)
            {
                mask = cValue << (bitsRemaining - 5);
                curByte = (byte)(curByte | mask);
                bitsRemaining -= 5;
            }
            else
            {
                mask = cValue >> (5 - bitsRemaining);
                curByte = (byte)(curByte | mask);
                returnArray[arrayIndex++] = curByte;
                curByte = (byte)(cValue << (3 + bitsRemaining));
                bitsRemaining += 3;
            }
        }

        if (arrayIndex != byteCount)
            returnArray[arrayIndex] = curByte;

        return returnArray;
    }

    private string ToBase32String(byte[] input)
    {
        if (input == null || input.Length == 0)
        {
            throw new ArgumentNullException("input");
        }

        int charCount = (int)Math.Ceiling(input.Length / 5d) * 8;
        char[] returnArray = new char[charCount];

        byte nextChar = 0, bitsRemaining = 5;
        int arrayIndex = 0;

        foreach (byte b in input)
        {
            nextChar = (byte)(nextChar | (b >> (8 - bitsRemaining)));
            returnArray[arrayIndex++] = ValueToChar(nextChar);

            if (bitsRemaining < 4)
            {
                nextChar = (byte)((b >> (3 - bitsRemaining)) & 31);
                returnArray[arrayIndex++] = ValueToChar(nextChar);
                bitsRemaining += 5;
            }

            bitsRemaining -= 3;
            nextChar = (byte)((b << bitsRemaining) & 31);
        }

        if (arrayIndex != charCount)
        {
            returnArray[arrayIndex++] = ValueToChar(nextChar);
            while (arrayIndex != charCount) returnArray[arrayIndex++] = '=';
        }

        return new string(returnArray);
    }

    private int CharToValue(char c)
    {
        int value = (int)c;

        if (value < 91 && value > 64)
            return value - 65;

        if (value < 56 && value > 49)
            return value - 24;

        if (value < 123 && value > 96)
            return value - 97;

        throw new ArgumentException("Character is not a Base32 character.", "c");
    }

    private char ValueToChar(byte b)
    {
        if (b < 26)
            return (char)(b + 65);

        if (b < 32)
            return (char)(b + 24);

        throw new ArgumentException("Byte is not a value Base32 value.", "b");
    }
}
