using System;
using System.Runtime.InteropServices;

namespace OS.Application
{
    public class NativeMethods
    {
        public const int HWND_BROADCAST = 0xffff;
        public static readonly int WM_CLOSE_ME = RegisterWindowMessage("WM_CLOSE_ME");
        public static readonly int WM_SHOWME = RegisterWindowMessage("WM_SHOWME");
        public static readonly int WM_SHOW_TRAY_POPUP = RegisterWindowMessage("WM_SHOW_TRAY_POPUP");
        [DllImport("user32")]
        public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);
        [DllImport("user32")]
        public static extern int RegisterWindowMessage(string message);
    }
}
