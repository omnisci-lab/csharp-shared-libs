﻿using System.Runtime.InteropServices;

namespace OmniSciLab.Console;

public class ConsoleMgr
{
    [DllImport("kernel32.dll")]
    public static extern bool AllocConsole();

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool FreeConsole();
}
