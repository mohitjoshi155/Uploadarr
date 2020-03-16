using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleInjector;
using Uploadarr.Common;

namespace Uploadarr.API
{
    public static class IoC
    {
        public static void RegisterContainers(ref Container container)
        {
            container.Register<IDiskProvider, DiskProviderBase>(Lifestyle.Singleton);
            container.Register<IFileSystemLookupService, FileSystemLookupService>(Lifestyle.Singleton);

            // Always verify the container
            container.Verify();
        }
    }
}
