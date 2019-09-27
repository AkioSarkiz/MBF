using MBF.Core.Config;
using MBF.Properties;
using System;
using System.IO;
using System.Net.Http;

namespace MBF.Core.Modules
{

    /// <summary>
    /// Класс обработчик POST данных от класса <see cref="PostPage"/>
    /// </summary>
    class TreatmentPostData
    {
        /// <summary>
        /// Шаблон ответа сервера в случае неудачной попытки
        /// </summary>
        private static string ErrorTemplate = File.ReadAllText(AppSettings.PathErrorTemplate); 

        /// <summary>
        /// Ответ сервера для обработки
        /// </summary>
        private HttpResponseMessage ResponseMessage;

        /// <summary>
        /// Контент ответа
        /// </summary>
        public String Content;

        /// <summary>
        /// Код ответа запроса
        /// </summary>
        public int Code;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="responseMessage">Ответ который нужно будет обрабатывать</param>
        public TreatmentPostData(HttpResponseMessage responseMessage)
        {
            this.Content = responseMessage.Content.ReadAsStringAsync().Result;
            this.Code = (int)responseMessage.StatusCode;
            this.ResponseMessage = responseMessage;
        }

        public bool Tests()
        {
            //if (AppSettings.TestInvalidTemplate)

            #if DEBUG
                Console.WriteLine("\n" + Resources.Log10, this.ResponseMessage.RequestMessage.RequestUri.ToString());
            #endif

          

            if (AppSettings.TestRedirect)
                if (this.ResponseMessage.RequestMessage.RequestUri.ToString().Equals(AppSettings.SuccessRedirect, StringComparison.CurrentCultureIgnoreCase))
                    return true;
            
            return false;
        }
    }
}
