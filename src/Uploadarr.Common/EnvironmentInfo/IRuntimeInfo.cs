using System;

namespace Uploadarr.Common
{
    public interface IRuntimeInfo
    {
        DateTime StartTime { get; }
        bool IsUserInteractive { get; }
        bool IsAdmin { get; }
        bool IsWindowsService { get; }
        bool IsWindowsTray { get; }
        bool IsExiting { get; set; }
        bool IsTray { get; }
        RuntimeMode Mode { get; }
        bool RestartPending { get; set; }
        string ExecutingApplication { get; }
    }
}
