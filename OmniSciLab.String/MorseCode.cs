using System.Text;

namespace OmniSciLab.String;

public class MorseCode
{
    private static Dictionary<char, string> _codes = new Dictionary<char, string>()
    {
        { 'A', ".-" }, { 'B', "-..." }, { 'C', "-.-"}, { 'D', "-.." }, { 'E', "." },
        { 'F', "..-." }, { 'G', "--." }, { 'H', "...." }, { 'I', ".." }, { 'J', ".---" },
        { 'K', "-.-" }, { 'L', ".-.." }, { 'M', "--" }, { 'N', "-." }, { 'O', "---" },
        { 'P', ".--." }, { 'Q', "--.-" }, { 'R', ".-." }, { 'S', "..." }, { 'T', "-" },
        { 'U', "..-" }, { 'V', "...-" }, { 'W', ".--" }, { 'X', "-..-" }, { 'Y', "-.--" },
        { 'Z', "--.." }, { '1', ".----" }, { '2', "..---" }, { '3', "...--" }, { '4', "....-" },
        { '5', "....." }, { '6', "-...." }, { '7', "--..." }, { '8', "---.." }, { '9', "----." },
        { '0', "-----" }
    };

    public static string ConvertFromText(string input)
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach(char character in input)
        {
            if (_codes.TryGetValue(character, out string? code))
                stringBuilder.Append(code);
            else
                stringBuilder.Append(character);
        }

        return stringBuilder.ToString();
    }
}
