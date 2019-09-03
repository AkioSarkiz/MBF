using MBF.Core.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MBF.Core.Request
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
            Task.Run(async () => this.Content = await responseMessage.Content.ReadAsStringAsync());
            this.Code = (int)responseMessage.StatusCode;
            this.ResponseMessage = responseMessage;
        }

        public bool Test()
        {

            return false;
        }
    }
}
