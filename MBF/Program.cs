using MBF.Core.Config;
using MBF.Core.Request;
using MBF.Properties;
using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
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
            if (!Init()) Environment.Exit(-1);

            ServicePointManager.DefaultConnectionLimit = int.MaxValue;
            Console.Title = "My Brute Force";
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;

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

            // поток запроса
            Thread requestThread = new Thread(() =>
            {
                GetPage getPage = new GetPage();
                getPage.Init().GetContent().Wait();

                PostPage postPage = new PostPage();
                postPage.Init().GetContent(getPage.Cookies, getPage.Token, "user", "user").Wait();
            });
            requestThread.Priority = ThreadPriority.Highest;
            requestThread.Start();

            Console.Read();
        }

        /// <summary>
        /// Создает папки по-умолчанию если их почему-то нет
        /// </summary>
        public static bool Init()
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
    }
}
