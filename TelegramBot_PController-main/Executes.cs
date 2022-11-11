using AudioSwitcher.AudioApi.CoreAudio;
using PController.Properties;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telegram.Bot;

namespace PController
{
    public class Executer
    {
        private TelegramBotClient telegram;

        // Mouse
        private static bool left = false;
        private static bool right = false;
        private static bool up = false;
        private static bool down = false;

        private static bool downUp = false;

        const int MOUSEPOS_CHANGE = 5;
        // Mouse

        public Executer(in TelegramBotClient telegram)
        {
            this.telegram = telegram;
        }

        public async void Start(long chatId)
        {
            if(Program.Admin <=0)
            {
                Program.Admin = chatId;
                Settings.Default["adminId"] = chatId;
                Settings.Default.Save();

                await telegram.SendTextMessageAsync(chatId, "Теперь вы мой хозяин.");
            }
            else
            {
                await telegram.SendTextMessageAsync(chatId,"Привееет!");
            }
        }

        public void Off()
        {
            Process.Start("cmd", "/c shutdown -s -f -t 00");
        }

        public void Reload()
        {
            Process.Start("shutdown", "/r /t 0");
        }
        public void Sleep()
        {
            Application.SetSuspendState(PowerState.Suspend, true, true);
        }

        public async void Screenshot(long chatId)
        {
            Bitmap printscreen = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics graphics = Graphics.FromImage(printscreen as Image);
            graphics.CopyFromScreen(0, 0, 0, 0, printscreen.Size);

            for (int j = Cursor.Position.X - 5; j < Cursor.Position.X + 5; j++)
                for (int i = Cursor.Position.Y - 5; i < Cursor.Position.Y + 5; i++)
                    printscreen.SetPixel(j, i, Color.Red);

            printscreen.Save($"D:\\screen.jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
            await telegram.SendPhotoAsync(chatId, System.IO.File.OpenRead($"D:\\screen.jpeg"));

            System.IO.File.Delete($"D:\\screen.jpeg");
        }

        public void Space()
        {
            SendKeys.SendWait(" ");
        }

        public void EnterText(string text)
        {
            text = Helpers.KeyboardtTextFormat(text.ToUpper());

            SendKeys.SendWait($"{text}");
        }

        public async void Volume(long chatId, int change)
        {
            CoreAudioDevice defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
            defaultPlaybackDevice.Volume = Helpers.ChangeBorder((int)defaultPlaybackDevice.Volume + change);
            await telegram.SendTextMessageAsync(chatId, $"Громкость теперь:  {defaultPlaybackDevice.Volume}");
        }

        public async void Brightness(long chatId, int change)
        {
            Brightness brightness = new Brightness();
            brightness.Set(Helpers.ChangeBorder(brightness.Get() + change));
            await telegram.SendTextMessageAsync(chatId, $"Яркость теперь:  {brightness.Get()}");
        }

        public void VideoForfard()
        {
            SendKeys.SendWait("{RIGHT}");
        }
        public void VideoBack()
        {
            SendKeys.SendWait("{LEFT}");
        }

        public async void Exit(long chatId)
        {
            await telegram.SendTextMessageAsync(chatId, "Пока...");
            Environment.Exit(0);
        }

        public async void Buttons(long chatId)
        {
            await telegram.SendTextMessageAsync(chatId, "Твои любимые кнопки:", replyMarkup: Program.GetControllButtons());
        }

        public async void Block(long chatId)
        {
            Program.Root = (chatId != Program.Admin);
            await telegram.SendTextMessageAsync(chatId, "Все пользователи заблокированы, Сэр");
        }

        public async void OnOffNotifications(long chatId)
        {
            if (chatId == Program.Admin)
            {
                Program.Notification = !Program.Notification;

                Settings.Default["notifications"] = Program.Notification;
                Settings.Default.Save();

                if(Program.Notification)
                    await telegram.SendTextMessageAsync(chatId, "Уведомления включены, Сер.");
                else
                    await telegram.SendTextMessageAsync(chatId, "Уведомления выключены, Сер.");
            }
        }

        public async void Combinations(long chatId)
        {
            await telegram.SendTextMessageAsync(chatId, "Так то по-удобнее будет)", replyMarkup: Program.GetСombinationsButtons());
        }

        public async void UnBlock(long chatId)
        {
            Program.Root = (chatId == Program.Admin);
            await telegram.SendTextMessageAsync(chatId, "Все пользователи разблокированы, Сэр");
        }

        public async void Mouse(long chatId)
        {
            await telegram.SendTextMessageAsync(chatId, "Управление перехвачено, Сэр", replyMarkup: Program.GetMouseButtons());
        }

        public void LeftClick()
        {
            MouseClick.LeftClick(Cursor.Position.X, Cursor.Position.Y);
        }
        public void LeftDown()
        {
            if (!downUp)
            {
                MouseClick.LeftDown(Cursor.Position.X, Cursor.Position.Y);
                downUp = !downUp;
            }
            else
            {
                MouseClick.LeftUp(Cursor.Position.X, Cursor.Position.Y);
                downUp = !downUp;
            }
        }
        public void RightClick()
        {
            MouseClick.RightClick(Cursor.Position.X, Cursor.Position.Y);
        }

        public async void MouseUp()
        {
            up = !up;

            while (up)
            {
                Cursor.Position = new Point(Cursor.Position.X,Cursor.Position.Y - MOUSEPOS_CHANGE);
                await Task.Delay(25);
            }
        }
        public async void MouseDown()
        {
            down = !down;

            while (down)
            {
                Cursor.Position = new Point(Cursor.Position.X,Cursor.Position.Y + MOUSEPOS_CHANGE);
                await Task.Delay(25);
            }
        }

        public async void DownloadFilm(long chatId, string name)
        {
            await telegram.SendTextMessageAsync(chatId, "Я уведомлю вас о начале и завершении заргузки фильма, Сер");

            var link = DownloadFilms.GetLink(name);

            if(link.Length > 0)
                await telegram.SendTextMessageAsync(chatId, $"Загрузка началась, Сэр");

            var web = new WebClient();
            await web.DownloadFileTaskAsync(link, $@"C:\Users\Администратор\Desktop\{name}.mp4");
            web.DownloadFileCompleted += async (s, e) => { await telegram.SendTextMessageAsync(chatId, "Скачалось, Сер"); };
        }

        public async void MouseLeft()
        {
            left = !left;

            while (left)
            {
                Cursor.Position = new Point(Cursor.Position.X - MOUSEPOS_CHANGE, Cursor.Position.Y);
                await Task.Delay(25);
            }
        }
        public async void MouseRight()
        {
            right = !right;

            while (right)
            {
                Cursor.Position = new Point(Cursor.Position.X + MOUSEPOS_CHANGE, Cursor.Position.Y);
                await Task.Delay(25);
            }
        }

        public void OpenRef(string command)
        {
            var parts = command.Split(' ');

            foreach (var part in parts)
                if (part.Contains("http"))
                {
                    Process.Start("browser.exe");
                    Process.Start(part);
                }
        }
    }
}
