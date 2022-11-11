using System;
using System.Reflection;
using Microsoft.Win32;

namespace PController
{
    public class AutoRun
    {
        public static void Auto_Run()
        {
            SetAutoRunValue(true, Assembly.GetExecutingAssembly().Location);
        }
        private static bool SetAutoRunValue(bool autorun, string path)
        {
            const string name = "PController";
            string exePath = path;

            RegistryKey reg;
            reg = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");

            try
            {
                if (autorun)
                    reg.SetValue(name, exePath);
                else
                    reg.DeleteValue(name);
                reg.Flush();
                reg.Close();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
