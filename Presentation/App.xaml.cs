using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Presentation.ViewModels;


namespace Presentation;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((services) =>
            {
                services.AddTransient<ProjectListViewModel>();
                services.AddSingleton<MainWindow>();
                services.AddSingleton<MainViewModel>();
            })
            .Build();
    }

}
