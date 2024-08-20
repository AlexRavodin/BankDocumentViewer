using System.Configuration;
using System.Data;
using System.Windows;
using BankDocumentViewer.Viewer.Data.DbContext;
using BankDocumentViewer.Viewer.Data.Options;
using BankDocumentViewer.Viewer.Services;
using BankDocumentViewer.Viewer.Stores;
using BankDocumentViewer.Viewer.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BankDocumentViewer.Viewer;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .AddServices()
            .AddViewModels()
            .ConfigureServices((hostContext, services) =>
            {
                var filesOptions = new FilesOptions();
                hostContext.Configuration.GetSection(nameof(FilesOptions)).Bind(filesOptions);

                filesOptions.TempDirPath = Path.Combine(Path.GetTempPath(), filesOptions.AppFolderName);
                services.AddSingleton(filesOptions);

                var generatingOptions = new GeneratingOptions();
                hostContext.Configuration.GetSection(nameof(GeneratingOptions)).Bind(generatingOptions);

                services.AddSingleton(generatingOptions);
                
                var connectionString = hostContext.Configuration.GetConnectionString("Default") ??
                                       throw new ConfigurationErrorsException("Connection string is not provided");
                services.AddSingleton<IAppDbContextFactory>(new AppDbContextFactory(connectionString));

                services.AddSingleton<NavigationStore>();

                services.AddSingleton(s => new MainWindow
                {
                    DataContext = s.GetRequiredService<MainViewModel>()
                });
            })
            .Build();
    }


    protected override void OnStartup(StartupEventArgs e)
    {
        NavigationService<MenuViewModel> navigationService =
            _host.Services.GetRequiredService<NavigationService<MenuViewModel>>();
        navigationService.Navigate();

        MainWindow = _host.Services.GetRequiredService<MainWindow>();
        MainWindow.Show();

        base.OnStartup(e);
    }
}