using MBF.Core.Config;
using MBF.Core.Modules;
using MBF.Core.Request;
using MBF.Properties;
using System;
using System.IO;
using System.Net;
using System.Threading;

namespace MBF
{
    /// <summary>
    /// Главный класс программы
    /// </summary>
    class Program
    {
        /// <summary>
        /// Начало цикла приложения
        /// </summary>
        /// <param name="args">аргументы команднной строки</param>
        static void Main(string[] args)
        {
            // Проверка и иницализация космпонентов
            if (!Init()) Environment.Exit(-1);
            InitThread();
            AppSettings.Init();
            TestArgs(args);

            // Настройки cmd
            ServicePointManager.DefaultConnectionLimit = int.MaxValue;
            Console.Title = "My Brute Force";
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;


            while (AppSettings.Run)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("123");
                Console.ForegroundColor = ConsoleColor.Black;

                // поток запроса
                Thread requestThread = new Thread(() =>
                {
                    GetPage getPage = new GetPage();
                    getPage.Init(Reader.GetProxy()).GetContent();

                    PostPage postPage = new PostPage();
                    postPage.Init(Reader.GetProxy()).GetContent(getPage.Cookies, getPage.Token, AppSettings.Login, Reader.GetPassword());
                });
                requestThread.Priority = ThreadPriority.Highest;
                requestThread.Start();

                Thread.Sleep(AppSettings.Delay);
            }

            Console.Read();
        }

        /// <summary>
        /// Создает папки по-умолчанию если их почему-то нет
        /// </summary>
        private static bool Init()
        {
            bool result = true;
            string self = AppDomain.CurrentDomain.BaseDirectory;
            if (!Directory.Exists(self + "config\\")) Directory.CreateDirectory(self + "config\\");

            // Create proxy.list
            if (!File.Exists(self + "config\\proxy.list"))
            {
                File.Open(self + "config\\proxy.list", FileMode.Create).Close();
                File.WriteAllText(self + "config\\proxy.list", Resources.Proxies);
                result = false;
            }

            // Create passwords.list
            if (!File.Exists(self + "config\\passwords.list"))
            {
                File.Open(self + "config\\passwords.list", FileMode.Create).Close();
                File.WriteAllText(self + "config\\passwords.list", Resources.Passwords);
                result = false;
            }

            // Create invalid.template
            if (!File.Exists(self + "config\\invalid.template"))
            {
                File.Open(self + "config\\invalid.template", FileMode.Create).Close();
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Создает базовые потоки приложения
        /// </summary>
        public static void InitThread()
        {
            // Поток для обновления логов
            Thread logsThread = new Thread(() =>
            {
                while (true)
                {
                    Loger.Update();
                }
            });
            logsThread.Name = "logsThread";
            logsThread.Priority = ThreadPriority.Highest;
            logsThread.Start();
        }

        /// <summary>
        /// Проверка аргементов командной строки
        /// </summary>
        /// <param name="args">аргументы</param>
        private static void TestArgs(string[] args)
        {
            if (args.Length > 0 && args[0].Equals("show-me", StringComparison.CurrentCulture))
            {
                char endl = '\n';
                Console.WriteLine(
                    "Login: " + AppSettings.Login + endl +
                    "BaseUrl: " + AppSettings.BaseUrl + endl +
                    "RequestUrl: " + AppSettings.RequestUrl + endl +
                    "SuccessRedirect: " + AppSettings.SuccessRedirect + endl +
                    "TestInvalidTemplate: " + AppSettings.TestInvalidTemplate + endl +
                    "TestRedirect: " + AppSettings.TestRedirect + endl +
                    "USE_PROXY: " + AppSettings.USE_PROXY + endl +
                    "WRITE_LOGS: " + AppSettings.WRITE_LOGS
                    );
                Environment.Exit(0);
            }
        }
    }
}
