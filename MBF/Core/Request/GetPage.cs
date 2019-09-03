using MBF.Core.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
        public GetPage Init(String adress = AppSettings.DefaultHost, int port = AppSettings.DefaultPort)
        {
            #if DEBUG
                Console.WriteLine("[GetPage.cs] Proxy: {0}:{1}", adress, port.ToString());
            #endif
            Proxy = new WebProxy(adress, port);
            return this;
        }

        /// <summary>
        /// Получение данных со страницы
        /// </summary>
        /// <returns></returns>
        public async Task<GetPage> GetContent()
        {
            
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() {
                CookieContainer = cookieContainer,
                Proxy = Proxy
            })
            using (var client = new HttpClient(handler))
            {
                // запрос
                var response = await client.GetAsync(this.RequestUrl);

                // Исключение если не удастся
                response.EnsureSuccessStatusCode();

                // получения контента страницы
                if (LoadContentPage)
                {
                    this.Content = await response.Content.ReadAsStringAsync();
                    #if DEBUG
                        Console.WriteLine("[GetPage.cs] Content: {0}", this.Content);
                    #endif
                }
                    

                // получение токена
                this.Token = new Regex("<meta name=\"_token\"\\s+content=\"[^\"]*").Match(await response.Content.ReadAsStringAsync()).ToString().Substring(29);
                #if DEBUG
                    Console.WriteLine("[GetPage.cs] Token: {0}", this.Token);
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
                        Console.WriteLine("[GetPage.cs] {0}: {1}", cookie.Name, cookie.Value);
                    #endif
                }    
            }

            return this;
        }
    }
}
