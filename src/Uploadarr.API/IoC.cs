using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.Reflection;
using Uploadarr.Common;
using Uploadarr.Core;
using Uploadarr.Data;
using ServiceProvider = Uploadarr.Common.ServiceProvider;

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
            services.AddSingleton<IServiceProvider, ServiceProvider>();
            services.AddSingleton<IFileSystemLookupService, FileSystemLookupService>();
            services.AddSingleton<IRuntimeInfo, RuntimeInfo>();
            services.AddSingleton<IProcessProvider, ProcessProvider>();

            // Services
            services.AddScoped<IRootFolderService, RootFolderService>();

            // Database context
            services.AddDbContext<MainDatabaseContext>();

            // Validators
            services.AddTransient<IValidator<RootFolderDTO>, RootFolderValidator>();

            services.AddAutoMapper(typeof(MappingProfile));
        }

    }
}
