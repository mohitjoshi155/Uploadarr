﻿using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Uploadarr.Common;

namespace Uploadarr.API
{
    public static class IoC
    {
        public static void RegisterContainers(IServiceCollection services, IWebHostEnvironment _hostingEnvironment)
        {
            var physicalProvider = _hostingEnvironment.ContentRootFileProvider;
            var embeddedProvider = new EmbeddedFileProvider(Assembly.GetEntryAssembly());
            var compositeProvider = new CompositeFileProvider(physicalProvider, embeddedProvider);

            // Dependency Injection
            services.AddSingleton<IFileProvider>(compositeProvider);
            services.AddSingleton<IDiskProvider, DiskProvider>();
        }
    }
}
