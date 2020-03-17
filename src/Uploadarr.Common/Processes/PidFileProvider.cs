using NLog;
using System;
using System.IO;

namespace Uploadarr.Common
{
    public interface IProvidePidFile
    {
        void Write();
    }

    public class PidFileProvider : IProvidePidFile
    {
        private readonly IAppFolderInfo _appFolderInfo;
        private readonly IProcessProvider _processProvider;
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public PidFileProvider(IAppFolderInfo appFolderInfo, IProcessProvider processProvider)
        {
            _appFolderInfo = appFolderInfo;
            _processProvider = processProvider;
        }

        public void Write()
        {
            if (OsInfo.IsWindows)
            {
                return;
            }

            var filename = Path.Combine(_appFolderInfo.AppDataFolder, "sonarr.pid");
            try
            {
                File.WriteAllText(filename, _processProvider.GetCurrentProcessId().ToString());
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unable to write PID file {0}", filename);
                throw new SonarrStartupException(ex, "Unable to write PID file {0}", filename);
            }
        }
    }
}
