using MBF.Core.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBF.Core.Request
{
    class Reader
    {
        public readonly string PathPasswords = AppSettings.PathPasswordFile;
        private static int Index = 0;
        private static string[] lines;

        public string GetPassword()
        {
            if (Index == 0) lines = File.ReadAllLines(PathPasswords);
            if(Index < lines.Length)
            {
                Index++;
                return lines[Index];
            }
            else
            {
                AppSettings.Run = false;
                return "TheEndProgram";
            }
        }
    }
}
