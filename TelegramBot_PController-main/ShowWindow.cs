using System;
using System.Runtime.InteropServices;

namespace PController
{
    public class Show
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        public static void ShowApp()
        {
            var handle = GetConsoleWindow();
            ShowWindow(handle, 0);
        }
    }
}
