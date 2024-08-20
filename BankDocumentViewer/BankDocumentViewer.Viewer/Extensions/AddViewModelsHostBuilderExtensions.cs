using BankDocumentViewer.Viewer.Services;
using BankDocumentViewer.Viewer.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BankDocumentViewer.Viewer.Extensions;

public static class AddViewModelsHostBuilderExtensions
{
    public static IHostBuilder AddViewModels(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureServices(services =>
        {
            services.AddTransient<MenuViewModel>();
            services.AddSingleton<Func<MenuViewModel>>(s => s.GetRequiredService<MenuViewModel>);
            services.AddSingleton<NavigationService<MenuViewModel>>();

            services.AddTransient<GenerateViewModel>();
            services.AddSingleton<Func<GenerateViewModel>>(s => s.GetRequiredService<GenerateViewModel>);
            services.AddSingleton<NavigationService<GenerateViewModel>>();

            services.AddTransient(CreateFilesListViewModel);
            services.AddSingleton<Func<FilesListViewModel>>(s => s.GetRequiredService<FilesListViewModel>);
            services.AddSingleton<NavigationService<FilesListViewModel>>();

            services.AddTransient(CreateOperationsListViewModel);
            services.AddSingleton<Func<OperationsListViewModel>>(s => s.GetRequiredService<OperationsListViewModel>);
            services.AddSingleton<NavigationService<OperationsListViewModel>>();

            services.AddSingleton<MainViewModel>();
        });

        return hostBuilder;
    }

    private static FilesListViewModel CreateFilesListViewModel(IServiceProvider services)
    {
        return FilesListViewModel.LoadViewModel(services.GetRequiredService<NavigationService<MenuViewModel>>(),
            services.GetRequiredService<NavigationService<OperationsListViewModel>>(),
            services.GetRequiredService<IDataProvider>(), 
            services.GetRequiredService<IBankDocumentParser>());
    }

    private static OperationsListViewModel CreateOperationsListViewModel(IServiceProvider services)
    {
        return OperationsListViewModel.LoadViewModel(
            services.GetRequiredService<NavigationService<FilesListViewModel>>(),
            services.GetRequiredService<IDataProvider>());
    }
}