using MBF.Core.Config;
using MBF.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace MBF.Core.Request
{
    /// <summary>
    /// Класс для получения данных с страницы
    /// </summary>
    class GetPage
    {
        private WebProxy Proxy;

        /// <summary>
        /// Загружать контент страницы в перменную Content или нет.
        /// Для ускорения загрузки эта переменная по-умолчанию false.
        /// При отладке можжно поставить на true, но не забудьте поменять
        /// </summary>
        public bool LoadContentPage = false;

        /// <summary>
        /// Токен полученный с ответа
        /// </summary>
        public string Token;

        /// <summary>
        /// Куки после выполнения запроса
        /// </summary>
        /// <remarks>Формат записи: "ключ"="значение";"ключ"="значение"</remarks>
        public Dictionary<string, string> Cookies = new Dictionary<string, string>();

        /// <summary>
        /// Возращаемый контент страницы
        /// </summary>
        public String Content;

        /// <summary>
        /// Url на который нужно делать запросы
        /// </summary>
        private readonly String RequestUrl = AppSettings.RequestUrl;

        /// <summary>
        /// Url на который нужно записывать куки
        /// </summary>
        private readonly String BaseUrl = AppSettings.BaseUrl;

        /// <summary>
        /// Инициализация объекта
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public GetPage Init(WebProxy proxy)
        {
            Proxy = (AppSettings.USE_PROXY) ? proxy : null;
            return this;
        }

        /// <summary>
        /// Инициализация объекта
        /// </summary>
        /// <returns></returns>
        public GetPage Init()
        {
            #if DEBUG
                Console.WriteLine(Resources.Log3, AppSettings.DefaultHost, AppSettings.DefaultPort.ToString(CultureInfo.CurrentCulture), AppSettings.USE_PROXY.ToString(CultureInfo.CurrentCulture));
            #endif
            Proxy = (AppSettings.USE_PROXY) ? new WebProxy(AppSettings.DefaultHost, AppSettings.DefaultPort) : null;
            return this;
        }

        /// <summary>
        /// Получение данных со страницы
        /// </summary>
        /// <returns></returns>
        public GetPage GetContent()
        {
            
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() {
                CookieContainer = cookieContainer,
                Proxy = Proxy
            })
            using (var client = new HttpClient(handler))
            {
                // запрос
                var response = client.GetAsync(new Uri(RequestUrl)).Result;

                // Исключение если не удастся
                response.EnsureSuccessStatusCode();

                // получения контента страницы
                if (LoadContentPage)
                {
                    this.Content = response.Content.ReadAsStringAsync().Result;
                    #if DEBUG
                        Console.WriteLine(Resources.Log4, Content);
                    #endif
                }
                    

                // получение токена
                this.Token = new Regex("<meta name=\"_token\"\\s+content=\"[^\"]*").Match(response.Content.ReadAsStringAsync().Result).ToString().Substring(29);
                #if DEBUG
                    Console.WriteLine(Resources.Log5, Token);
                #endif

                /**************************************
                 * Сохранение куков в перменную
                 **************************************/
                var cookies = handler.CookieContainer
                    .GetCookies(new Uri(this.BaseUrl));

                foreach (Cookie cookie in cookies)
                {
                    Cookies.Add(cookie.Name, cookie.Value);
                    #if DEBUG
                        Console.WriteLine(Resources.Log1, cookie.Name, cookie.Value);
                    #endif
                }    
            }

            return this;
        }
    }
}
