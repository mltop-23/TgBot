namespace PController
{
    public class Helpers
    {
       public static int ChangeBorder(int change)
        {
            if (change > 100)
                change = 100;
            if (change < 0)
                change = 0;
            return change;
        }

        public static string KeyboardtTextFormat(string text)
        {
            return text.Replace("BACKSPACE", "{BACKSPACE}").Replace("DEL", "{DEL}").Replace("ENTER", "{ENTER}").Replace("ESC", "{ESC}").Replace("TAB","{TAB}").Replace("SHIFT","+").Replace("CTRL","^").Replace("ALT","%").Replace("F1","{F1}").Replace("F2", "{F2}").Replace("F3", "{F3}").Replace("F4", "{F4}").Replace("F5", "{F5}").Replace("F6", "{F6}").Replace("F7", "{F7}").Replace("F8", "{F8}").Replace("F9", "{F9}").Replace("F10", "{F10}").Replace("F11", "{F11}").Replace("F12", "{F12}").ToLower();
        }
    }
}
