using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBF.Core.Config
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
            if (!Directory.Exists(Path)) Directory.CreateDirectory(Path);
            if (!File.Exists(Path + FileNameLog)) File.Create(Path + FileNameLog);
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + FileNameSucc)) File.Create(AppDomain.CurrentDomain.BaseDirectory + FileNameSucc);

            try
            {
                using (var sw = File.AppendText(Path + FileNameLog))
                {
                    sw.WriteLine(string.Format("[{0}] {1}", DateTime.Now.ToString(), Logs[0]));
                    Logs.Remove(Logs[0]);
                }
            }
            catch { }

            try
            {
                using(var sw = File.AppendText(AppDomain.CurrentDomain.BaseDirectory + FileNameSucc))
                {
                    sw.WriteLine(string.Format("[{0}] {1}", DateTime.Now.ToString(), LogsSucc[0]));
                    LogsSucc.Remove(LogsSucc[0]);
                }
            }
            catch { }
        }
    }
}
