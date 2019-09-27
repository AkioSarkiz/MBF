using MBF.Core.Modules;
using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace MBF.Core.Config
{
    class AppSettings
    {
        /// <summary>
        /// Конструктор
        /// Подгружает парамерты из каталога [self]/config/AppSettings.txt
        /// </summary>
        public static void Init()
        {
            string self = AppDomain.CurrentDomain.BaseDirectory;
            if (File.Exists(self + @"config\AppSettings.txt"))
            {
                string[] lines = File.ReadAllLines(self + @"config\AppSettings.txt");

                foreach (string line in lines)
                {
                    if (line.Length == 0 || line.ToCharArray()[0] == '#')
                        continue;   // поропускаем строки комментарии

                    var matchCollection = new Regex(@"[^=\s]+").Matches(line);

                    string key = matchCollection[0].ToString();
                    string value = matchCollection[1].ToString();

                    switch (key)
                    {
                        case "Login":
                            if (value.Length > 0)
                                Login = value;
                            break;

                        case "DefaultHost":
                            DefaultHost = value;
                            break;

                        case "DefaultPort":
                            try
                            {
                                DefaultPort = Convert.ToInt16(value, CultureInfo.CurrentCulture);
                            }
                            catch (FormatException e) { Loger.AddLog("error set param DefaultPort in AppSettings.cs" + Environment.NewLine + "info: " + e); }
                            break;   
                            
                        case "BaseUrl":
                            BaseUrl = value;
                            break;

                        case "RequestUrl":
                            RequestUrl = value;
                            break;

                        case "WRITE_LOGS":
                            try
                            {
                                WRITE_LOGS = Convert.ToBoolean(value, CultureInfo.CurrentCulture);
                            }
                            catch (FormatException e) { Loger.AddLog("error set param WRITE_LOGS in AppSettings.cs" + Environment.NewLine + "info: " + e); }
                            break;

                        case "USE_PROXY":
                            try
                            {
                                USE_PROXY = Convert.ToBoolean(value, CultureInfo.CurrentCulture);
                            }
                            catch (FormatException e) { Loger.AddLog("error set param USE_PROXY in AppSettings.cs" + Environment.NewLine + "info: " + e); }
                            break;

                        case "Delay":
                            try
                            {
                                Delay = Convert.ToInt16(value, CultureInfo.CurrentCulture);
                            }
                            catch (FormatException e) { Loger.AddLog("error set param Delay in AppSettings.cs" + Environment.NewLine + "info: " + e); }
                            break;

                        case "TestInvalidTemplate":
                            try
                            {
                                TestInvalidTemplate = Convert.ToBoolean(value, CultureInfo.CurrentCulture);
                            }
                            catch (FormatException e) { Loger.AddLog("error set param TestInvalidTemplate in AppSettings.cs" + Environment.NewLine + "info: " + e); }
                            break;

                        case "TestRedirect":
                            try
                            {
                                TestRedirect = Convert.ToBoolean(value, CultureInfo.CurrentCulture);
                            }
                            catch(FormatException e) { Loger.AddLog("error set param TestRedirect in AppSettings.cs" + Environment.NewLine + "info: " + e); }
                            break;

                        case "SuccessRedirect":
                            SuccessRedirect = value;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Логин
        /// </summary>
        public static string Login = "testCrack";

        /// <summary>
        /// Ошибочнный шаблон
        /// </summary>
        public static string PathErrorTemplate = AppDomain.CurrentDomain.BaseDirectory + @"config\invalid.template";

        /// <summary>
        /// Значение по-умолчанию для хоста и порта
        /// </summary>
        public static string DefaultHost = "34.94.15.111";
        public static int DefaultPort = 8080;

        /// <summary>
        /// Путь к файлу с паролями
        /// </summary>
        public static string PathPasswordFile = AppDomain.CurrentDomain.BaseDirectory + @"config\passwords.list";

        /// <summary>
        /// Путь к файлу с прокси
        /// </summary>
        public static string PathProxyFile = AppDomain.CurrentDomain.BaseDirectory + @"config\proxy.list";

        /// <summary>
        /// Базовый Url для установки куки и прочего
        /// </summary>
        public static string BaseUrl = "https://mangalib.me";

        /// <summary>
        /// Url для запроса на сервер
        /// </summary>
        public static string RequestUrl = "https://mangalib.me/login";

        /// <summary>
        /// Указывает на состояние приложения
        /// </summary>
        public static bool Run = true;

        /// <summary>
        /// Записывать ли логи
        /// </summary>
        public static bool WRITE_LOGS = true;

        /// <summary>
        /// Использовать ли прокси
        /// </summary>
        public static bool USE_PROXY = false;

        /// <summary>
        /// Задержка запросов
        /// </summary>
        public static int Delay = 100;

        /// <summary>
        /// URL удачного перехода
        /// </summary>
        public static string SuccessRedirect = "https://mangalib.me/";

        /// <summary>
        /// Проверять ли переход
        /// </summary>
        public static bool TestRedirect = true;

        /// <summary>
        /// Проверять ли контент страницы
        /// </summary>
        public static bool TestInvalidTemplate = false;
    }
}
