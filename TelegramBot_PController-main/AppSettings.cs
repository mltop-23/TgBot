namespace PController
{
    public class AppSettings
    {
        static AppSettings()
        {
            Autorun();
            //ShowApp();
        }

        private static void Autorun()
        {
            AutoRun.Auto_Run();
        }

        private static void ShowApp()
        {
            Show.ShowApp();
        }
    }
}
