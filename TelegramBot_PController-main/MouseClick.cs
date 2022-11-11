using System.Runtime.InteropServices;

namespace PController
{
    public enum MouseEvent
    {
        MOUSEEVENTF_LEFTDOWN = 0x02,
        MOUSEEVENTF_LEFTUP = 0x04,
        MOUSEEVENTF_RIGHTDOWN = 0x08,
        MOUSEEVENTF_RIGHTUP = 0x10,
    }
    public class MouseClick
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(MouseEvent dwFlags, long dx, long dy, long cButtons, long dwExtraInfo);



        public static void LeftClick(int x, int y)
        {
            mouse_event(MouseEvent.MOUSEEVENTF_LEFTDOWN, x, y, 0, 0);
            mouse_event(MouseEvent.MOUSEEVENTF_LEFTUP, x, y, 0, 0);
        }
        public static void LeftDown(int x, int y)
        {
            mouse_event(MouseEvent.MOUSEEVENTF_LEFTDOWN, x, y, 0, 0);
        }
        public static void LeftUp(int x, int y)
        {
            mouse_event(MouseEvent.MOUSEEVENTF_LEFTUP, x, y, 0, 0);
        }
        public static void RightClick(int x, int y)
        {
            mouse_event(MouseEvent.MOUSEEVENTF_RIGHTDOWN, x, y, 0, 0);
            mouse_event(MouseEvent.MOUSEEVENTF_RIGHTUP, x, y, 0, 0);
        }

    }
}
