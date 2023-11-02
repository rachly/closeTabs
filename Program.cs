using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

class Program
{
    static void Main(string[] args)
    {
        // Specify the URL to match
        string urlToMatch = "http://localhost:52431/api/Users/getUsersSessionsbyDay";

        // Find and close Chrome tabs with the specified URL
        CloseChromeTabsWithUrl(urlToMatch);

        Console.WriteLine("Chrome tabs with the specified URL are closed.");
    }

    static void CloseChromeTabsWithUrl(string urlToMatch)
    {
        Process[] chromeProcesses = Process.GetProcessesByName("chrome");

        foreach (Process chromeProcess in chromeProcesses)
        {
            chromeProcess.CloseMainWindow();
          
        }
    }

    static string GetTabUrl(IntPtr hwnd)
    {
        const int maxChars = 2048;
        IntPtr buffer = IntPtr.Zero;

        try
        {
            buffer = Marshal.AllocHGlobal(maxChars);
            if (Marshal.ReadInt32(NativeMethods.SendMessage(hwnd, NativeMethods.WM_GETTEXT, (IntPtr)maxChars, buffer)) != 0)
            {
                return Marshal.PtrToStringAuto(buffer);
            }
        }
        finally
        {
            if (buffer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        return null;
    }
}

internal static class NativeMethods
{
    public const int WM_CLOSE = 0x0010;
    public const int WM_GETTEXT = 0x000D;

    [DllImport("user32.dll")]
    public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

    [DllImport("user32.dll")]
    public static extern IntPtr PostMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
}
