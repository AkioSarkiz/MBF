using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace MBF.Core.Modules
{
    class Loger
    {
        private static readonly string Path = AppDomain.CurrentDomain.BaseDirectory + "logs\\";
        private static readonly string FileNameLog = "log_" + DateTime.Today.Day + "_" + DateTime.Today.Month + ".txt";
        private static List<string> Logs = new List<string>();

        private static readonly string FileNameSucc = "Success_" + DateTime.Today.Day + "_" + DateTime.Today.Month + ".txt";
        private static List<string> LogsSucc = new List<string>();


        public static void AddSuccess(string add)
        {
            LogsSucc.Add(add);
        }

        public static void AddLog(string add)
        {
            Logs.Add(add);
        }

        public static void Update()
        {
            // проверка наличия файлов и папки
            if (!Directory.Exists(Path)) Directory.CreateDirectory(Path);
            if (!File.Exists(Path + FileNameLog)) File.Create(Path + FileNameLog).Close();
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + FileNameSucc)) File.Create(AppDomain.CurrentDomain.BaseDirectory + FileNameSucc).Close();
            
            if (Logs.Count > 0)
            {
                using (var sw = File.AppendText(Path + FileNameLog))
                {
                    sw.WriteLine(format: "[{0}] {1}", DateTime.Now.ToString(CultureInfo.CurrentCulture), Logs[0]);
                    Logs.Remove(Logs[0]);
                }
            }
            
            if(LogsSucc.Count > 0)
            {
                using(var sw = File.AppendText(AppDomain.CurrentDomain.BaseDirectory + FileNameSucc))
                {
                    sw.WriteLine("[{0}] {1}", DateTime.Now.ToString(CultureInfo.CurrentCulture), LogsSucc[0]);
                    LogsSucc.Remove(LogsSucc[0]);
                }
            }
        }
    }
}
