namespace OmniSciLab.Console;

public partial class Argument
{
    public string ArgName { get; set; } = default!;
    public Arguments ArgType { get; set; }
}

public partial class Argument
{
    private static string[] _args = null!;
    private static readonly HashSet<string> _positionalArgs = new HashSet<string>();
    private static readonly Dictionary<string, string> _options = new Dictionary<string, string>();
    private static readonly HashSet<string> _flags = new HashSet<string>();
    private static bool _loaded = false;

    public static void Load(string[] args)
    {
        if (_loaded == true)
            return;

        _args = args;

        for (int i = 0; i < _args.Length; i++)
        {
            if (_args[i].StartsWith("--"))
            {
                // Named Argument / Option Argument
                if (i + 1 < _args.Length && !_args[i + 1].StartsWith("--") && !_args[i + 1].StartsWith("-"))
                {
                    _options.Add(_args[i], _args[i + 1]);
                    i++;
                }
                else
                {
                    // Flag Argument
                    _flags.Add(_args[i]);
                }
            }
            else if (_args[i].StartsWith("-"))
            {
                // Short Option Argument
                string key = _args[i].Substring(1);
                if (i + 1 < _args.Length && !_args[i + 1].StartsWith("--") && !_args[i + 1].StartsWith("-"))
                {
                    _options.Add(_args[i], _args[i + 1]);
                    i++;
                }
                else
                {
                    // Flag Argument
                    _flags.Add(_args[i]);
                }
            }
            else
            {
                // Positional Argument
                _positionalArgs.Add(_args[i]);
            }
        }

        _loaded = true;
    }

    public static bool IsEmpty { get => _args.Length == 0; }

    public static bool Check(params Argument[] arguments)
    {
        if (arguments is null)
            throw new ArgumentNullException(nameof(arguments));

        if(_loaded == false)
            throw new InvalidOperationException("Arguments not loaded");

        if (arguments.Length == 0)
            return _args.Length == arguments.Length;

        if (arguments.Length != _positionalArgs.Count + _flags.Count + _options.Count)
            return false;

        if (_positionalArgs.Count != arguments.Count(s => s.ArgType == Arguments.Positional))
            return false;

        if (_options.Count != arguments.Count(s => s.ArgType == Arguments.Option))
            return false;

        if (_flags.Count != arguments.Count(s => s.ArgType == Arguments.Flag))
            return false;

        bool valid = true;

        foreach (var arg in arguments)
        {
            if (arg.ArgType == Arguments.Positional)
                valid = valid && _positionalArgs.Contains(arg.ArgName);
            else if (arg.ArgType == Arguments.Option)
                valid = valid && _options.ContainsKey(arg.ArgName);
            else if (arg.ArgType == Arguments.Flag)
                valid = valid && _flags.Contains(arg.ArgName);
        }

        return valid;
    }

    public static string? GetOptionArg(string argument)
    {
        if (string.IsNullOrEmpty(argument))
            throw new ArgumentNullException(nameof(argument));

        return _options.ContainsKey(argument) ? _options[argument] : null;
    }

    public static Argument Positional(string arg) => new Argument { ArgName = arg, ArgType = Arguments.Positional };
    public static Argument Option(string arg) => new Argument { ArgName = arg, ArgType = Arguments.Option };
    public static Argument Flag(string arg) => new Argument { ArgName = arg, ArgType = Arguments.Flag };
}