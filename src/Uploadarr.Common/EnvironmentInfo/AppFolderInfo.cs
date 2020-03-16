using System;
using System.IO;
using System.Reflection;
using NLog;

namespace Uploadarr.Common
{
    public interface IAppFolderInfo
    {
        string AppDataFolder { get; }
        string LegacyAppDataFolder { get; }
        string TempFolder { get; }
        string StartUpFolder { get; }
    }

    public class AppFolderInfo : IAppFolderInfo
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private readonly Environment.SpecialFolder DATA_SPECIAL_FOLDER = Environment.SpecialFolder.CommonApplicationData;


        public AppFolderInfo(IStartupContext startupContext)
        {
            if (OsInfo.IsNotWindows)
            {
                DATA_SPECIAL_FOLDER = Environment.SpecialFolder.ApplicationData;
            }

            if (startupContext.Args.ContainsKey(StartupContext.APPDATA))
            {
                AppDataFolder = startupContext.Args[StartupContext.APPDATA];
                Log.Info("Data directory is being overridden to [{0}]", AppDataFolder);
            }
            else
            {
                AppDataFolder = Path.Combine(Environment.GetFolderPath(DATA_SPECIAL_FOLDER, Environment.SpecialFolderOption.None), "Sonarr");
                LegacyAppDataFolder = Path.Combine(Environment.GetFolderPath(DATA_SPECIAL_FOLDER, Environment.SpecialFolderOption.None), "NzbDrone");
            }

            StartUpFolder = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            TempFolder = Path.GetTempPath();
        }

        public string AppDataFolder { get; }
        public string LegacyAppDataFolder { get; }
        public string StartUpFolder { get; }
        public string TempFolder { get; }
    }
}
