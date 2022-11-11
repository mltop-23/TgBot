using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using PController.Properties;

namespace PController
{
    class Program
    {
        public static TelegramBotClient Telegram { get; private set; }

        public static bool Root { get; set; }
        public static bool Notification { get; set; }
        public static long Admin { get; set; }

        static void Main(string[] args)
        {
            if (Settings.Default["token"].ToString().Length == 0)
            {
                Console.WriteLine("Зайди в телеграм и найди там отца всех ботов 'BotFather'\nНажми на '/newbot'\nДай ему имя и еще имя, где в конце 'bot'\nИ опа ля, у тебя есть токен\nВпиши его сюда");
                Settings.Default["token"] = Console.ReadLine();
                Settings.Default.Save();
            }

            if (Settings.Default["token"].ToString().Length > 0)
            {
                new AppSettings();

                Root = true;
                if (Settings.Default["adminId"] != null)
                    Admin = long.Parse(Settings.Default["adminId"].ToString());
                Notification = bool.Parse(Settings.Default["notifications"].ToString());

                while (Telegram == null) { 
                    Telegram = new TelegramBotClient(Settings.Default["token"].ToString());
                    Task.Delay(1000).Wait();
                }

                GetMessages().Wait();
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Что-то не так с токеном, брат");
                Console.ReadLine();
            }
        }


        private static async Task GetMessages()
        {
            if (Notification && Admin > 0)
            {
                await Telegram.SendTextMessageAsync(Admin, "Хай!");
                AppDomain.CurrentDomain.ProcessExit += async (s, e) => { await Telegram.SendTextMessageAsync(Admin, "Пока!"); };
            }

            while (true)
            {
                try 
                {
                    var updates = await Telegram.GetUpdatesAsync(offset: int.Parse(Settings.Default["offset"].ToString()), timeout: 30);

                    if (updates.Length != 0)
                    {
                        foreach (var update in updates)
                        {
                            Settings.Default["offset"] = update.Id + 1;
                            Settings.Default.Save();

                            if (Root || update.Message.Chat.Id == Admin)
                                Execute(update);
                            else
                                await Telegram.SendTextMessageAsync(update.Message.Chat.Id, "Хрен тебе");
                        }
                    }
                    Task.Delay(10).Wait();
                }
                catch
                {
                    Task.Delay(1000).Wait();
                    continue;
                }
            }
            

        }

        private static void Execute(Update message)
        {
            var command = message.Message.Text;
            var chatId = message.Message.Chat.Id;

            var executer = new Executer(Telegram);

            switch (command)
            {
                case "/start":
                    executer.Start(chatId);
                    break;
                case "/buttons":
                    executer.Buttons(chatId);
                    break;
                case "/off":
                    executer.Off();
                    break;
                case "/reload":
                    executer.Reload();
                    break;
                case "/sleep":
                    executer.Sleep();
                    break;
                case "/screenshot":
                    executer.Screenshot(chatId);
                    break;
                case "/combinations":
                    executer.Combinations(chatId);
                    break;
                case "/notifications":
                    executer.OnOffNotifications(chatId);
                    break;
                case "Громкость\n+5":
                    executer.Volume(chatId: chatId, change: +5);
                    break;
                case "Громкость\n-5":
                    executer.Volume(chatId: chatId, change: -5);
                    break;
                case "Яркость\n+10":
                    executer.Brightness(chatId: chatId, change: +10);
                    break;
                case "Яркость\n-10":
                    executer.Brightness(chatId: chatId, change: -10);
                    break;
                case "Перемотать\nвперёд":
                    executer.VideoForfard();
                    break;
                case "Перемотать\nназад":
                    executer.VideoBack();
                    break;
                case "Пробел":
                    executer.Space();
                    break;
                case "/block":
                    executer.Block(chatId);
                    break;
                case "/exit":
                    executer.Exit(chatId);
                    break;
                case "/unblock":
                    executer.UnBlock(chatId);
                    break;
                case "/mouse":
                    executer.Mouse(chatId);
                    break;
                case "Левый\nклик":
                    executer.LeftClick();
                    break;
                case "Правый\nклик":
                    executer.RightClick();
                    break;
                case "Зажать":
                    executer.LeftDown();
                    break;
                case "Назад":
                    executer.Start(chatId);
                    break;
                case "↑":
                    executer.MouseUp();
                    break;
                case "←":
                    executer.MouseLeft();
                    break;
                case "→":
                    executer.MouseRight();
                    break;
                case "↓":
                    executer.MouseDown();
                    break;
                default:

                    if (command.Contains("http"))
                        executer.OpenRef(command);
                    else if (command[0] == '*')
                        executer.DownloadFilm(chatId,command.Substring(1));
                    else
                        executer.EnterText(command);
                    break;
            }
        }

        public static IReplyMarkup GetControllButtons()
        {
            return new ReplyKeyboardMarkup(new List<List<KeyboardButton>>(){
                    new List<KeyboardButton>{ new KeyboardButton("Громкость\n+5"), new KeyboardButton("Громкость\n-5") },
                    new List<KeyboardButton>{ new KeyboardButton ("Яркость\n+10"), new KeyboardButton ("Яркость\n-10") },
                    new List<KeyboardButton>{ new KeyboardButton ("Перемотать\nназад"), new KeyboardButton ("Перемотать\nвперёд") },
                    new List<KeyboardButton>{ new KeyboardButton ("Пробел") }
                });
        }
        public static IReplyMarkup GetMouseButtons()
        {
            return new ReplyKeyboardMarkup(new List<List<KeyboardButton>>(){
                    new List<KeyboardButton>{ new KeyboardButton("Левый\nклик"), new KeyboardButton("↑"), new KeyboardButton("Правый\nклик") },
                    new List<KeyboardButton>{ new KeyboardButton ("←"), new KeyboardButton ("→") },
                    new List<KeyboardButton>{ new KeyboardButton("Зажать"), new KeyboardButton("↓"), new KeyboardButton("Назад") }
                });
        }
        public static IReplyMarkup GetСombinationsButtons()
        {
            return new ReplyKeyboardMarkup(new List<List<KeyboardButton>>(){
                    new List<KeyboardButton>{ new KeyboardButton("CtrlA"), new KeyboardButton("CtrlC"), new KeyboardButton("CtrlV") },
                    new List<KeyboardButton>{ new KeyboardButton("AltF4"), new KeyboardButton("CtrlW"), new KeyboardButton("Enter") },
                    new List<KeyboardButton>{ new KeyboardButton("CtrlT"), new KeyboardButton("CtrlShiftT"), new KeyboardButton("CtrlH") },
                    new List<KeyboardButton>{ new KeyboardButton("AltTab"), new KeyboardButton("CtrlZ"), new KeyboardButton("Backspace") }
                });
        }
    }
}
