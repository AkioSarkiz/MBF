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
    /// Класс для отправки данных
    /// </summary>
    class PostPage
    {
        /// <summary>
        /// Url на который нужно делать запросы
        /// </summary>
        private readonly String RequestUrl = AppSettings.RequestUrl;

        /// <summary>
        /// Url на который нужно записывать куки
        /// </summary>
        private readonly String BaseUrl = AppSettings.BaseUrl;

        /// <summary>
        /// Прокси для запроса
        /// </summary>
        public WebProxy Proxy;

        /// <summary>
        /// Контент после отправки данных
        /// </summary>
        public string Content;

        /// <summary>
        /// Инициализация объекта
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public PostPage Init(String adress = AppSettings.DefaultHost, int port = AppSettings.DefaultPort)
        {
            #if DEBUG
                Console.WriteLine("[PostPage.cs] Proxy: {0}:{1}", adress, port.ToString());
            #endif
            Proxy = new WebProxy(adress, port);
            return this;
        }

        public async Task<HttpResponseMessage> GetContent(Dictionary<string, string> cookies, string _token, string login, string password)
        {

            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                CookieContainer = cookieContainer,
                Proxy = Proxy
            })

            using (var client = new HttpClient(handler))
            {
                // установка заголовков
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
                client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
                client.DefaultRequestHeaders.Add("Accept-Language", "am,uk;q=0.7,ru;q=0.3");
                client.DefaultRequestHeaders.Add("Host", "mangalib.me");
                client.DefaultRequestHeaders.Add("Referer", "https://mangalib.me/login");
                client.DefaultRequestHeaders.Add("TE", "Trailers");
                client.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.3; Win64; x64; rv:69.0) Gecko/20100101 Firefox/69.0");

                // POST данные для отправки
                var postData = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("email", login),
                    new KeyValuePair<string, string>("password", password),
                    new KeyValuePair<string, string>("_token", _token)
                });
                #if DEBUG
                    Console.WriteLine("[PostPage.cs] Post data: email: {0}\tpassword: {1}\t _token: {2}", login, password, _token);
                #endif


                // Установка куки
                foreach (var obj in cookies)
                {
                    cookieContainer.Add(new Uri(BaseUrl), new Cookie(obj.Key, obj.Value));
                    #if DEBUG
                        Console.Write("[PostPage.cs] Cokie name: {0}\n[PostPage.cs] Cookie value: {1}\n\n", obj.Key, obj.Value);
                    #endif
                }

                // запрос
                HttpResponseMessage response = client.PostAsync(RequestUrl, postData).Result;
                Content = response.Content.ReadAsStringAsync().Result;

                foreach (var b in response.Content.ReadAsByteArrayAsync().Result)
                    Console.Write(b + " ");

                #if DEBUG
                    Console.WriteLine("[PostPage.cs] Content: {0}",response.Content.ReadAsStringAsync().Result);
                #endif
                return response;
            }
        }
    }
}
