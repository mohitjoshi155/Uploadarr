using System;

namespace Uploadarr.Common
{
    public static class LongPathSupport
    {
        public static void Enable()
        {
            // Mono has an issue with enabling long path support via app.config.
            // This works for both mono and .net on Windows.
            AppContext.SetSwitch("Switch.System.IO.UseLegacyPathHandling", false);
            AppContext.SetSwitch("Switch.System.IO.BlockLongPaths", false);
        }
    }
}
