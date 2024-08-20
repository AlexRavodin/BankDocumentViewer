using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Viewer.Models;
using Viewer.Services;

namespace Viewer.Extensions;

public static class AddServicesBuilderExtensions
{
    public static IHostBuilder AddServices(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureServices(services =>
        {
            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<IDataGenerator, DataGenerator>();
            services.AddSingleton<IDataProvider, DataProvider>();
            services.AddSingleton<IBankDocumentParser, XlsBankDocumentParser>();
        });

        return hostBuilder;
    }
}