using MBF.Core.Config;
using System.IO;

namespace MBF.Core.Request
{
    class Proxy
    {
        public static string ProxyFile = AppSettings.PathProxyFile;
        private static string[] Lines;
        private static int Index = 0;

        public static string GetProxy()
        {
            if (Index == 0) Lines = File.ReadAllLines(ProxyFile);
            if (Index < Lines.Length)
            {
                Index++;
                return Lines[Index];
            }
            else
            {
                Index = 0;
                return Lines[Index];
            }    
        } 
    }
}
