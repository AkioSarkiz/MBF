using MBF.Core.Config;
using System.IO;
using System.Net;

namespace MBF.Core.Modules
{
    class Reader
    {
        private static readonly string PathProxies = AppSettings.PathProxyFile;
        private static readonly string PathPasswords = AppSettings.PathPasswordFile;
        private static int Index = 0;
        private static string[] Lines;
        private static string[] Proxies;
        private static int IndexProxy = -1;

        public static WebProxy GetProxy()
        {
            if (Proxies == null)
                Proxies = File.ReadAllLines(PathProxies);

            IndexProxy++;
            if (IndexProxy == Proxies.Length || Proxies[IndexProxy].Length == 0)
                IndexProxy = 0;

            return new WebProxy(Proxies[IndexProxy]);
        }

        public static string GetPassword()
        {
            if (Index == 0) Lines = File.ReadAllLines(PathPasswords);
            Index++;
            if (Index < Lines.Length)
            {
                return Lines[Index];
            }
            else
            {
                AppSettings.Run = false;
                return "TheEndProgram";
            }
        }
    }
}
