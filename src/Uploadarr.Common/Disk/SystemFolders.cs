using System;
using System.Collections.Generic;

namespace Uploadarr.Common
{
    public static class SystemFolders
    {
        public static List<string> GetSystemFolders()
        {
            if (OsInfo.IsWindows)
            {
                return new List<string> { Environment.GetFolderPath(Environment.SpecialFolder.Windows) };
            }

            if (OsInfo.IsOsx)
            {
                return new List<string> { "/System" };
            }

            return new List<string>
                   {
                       "/bin",
                       "/boot",
                       "/lib",
                       "/sbin",
                       "/proc"
                   };
        }
    }
}
