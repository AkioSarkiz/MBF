namespace MBF.Core.Config
{
    class AppSettings
    {
        public static string PathErrorTemplate;

        /// <summary>
        /// Значение по-умолчанию для хоста и порта
        /// </summary>
        public const string DefaultHost = "142.44.242.38";
        public const int DefaultPort = 8888;

        /// <summary>
        /// Путь к файлу с паролями
        /// </summary>
        public static string PathPasswordFile;

        /// <summary>
        /// Путь к файлу с прокси
        /// </summary>
        public static string PathProxyFile; 

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
        public const bool WRITE_LOGS = true;
    }
}
