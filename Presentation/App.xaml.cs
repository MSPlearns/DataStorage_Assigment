using System.Windows;
using Business.Mappers;
using Business.Services;
using Data.Context;
using Data.Interfaces;
using Data.Repositories;
using Domain.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Presentation.ViewModels;
using Presentation.Views;


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
                services.AddDbContext<DataContext>(options =>
                    options.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\WIN24\\Y1\\4.Datalagring\\Data\\DataStorage_Assigment\\Data\\Databases\\local_database.mdf;Integrated Security=True;Connect Timeout=30"));


                services.AddTransient<ICustomerRepository, CustomerRepository>();
                services.AddTransient<IProjectRepository, ProjectRepository>();
                services.AddTransient<IUserRepository, UserRepository>();
                services.AddTransient<IProductRepository, ProductRepository>();

                services.AddTransient<ICustomerFactory, CustomerFactory>();
                services.AddTransient<IProjectFactory, ProjectFactory>();
                services.AddTransient<IUserFactory, UserFactory>();
                services.AddTransient<IProductFactory, ProductFactory>();

                services.AddSingleton<ICustomerMapper, CustomerMapper>();
                services.AddSingleton<IProjectMapper, ProjectMapper>();
                services.AddSingleton<IUserMapper, UserMapper>();
                services.AddSingleton<IProductMapper, ProductMapper>();

                services.AddTransient<ICustomerService, CustomerService>();
                services.AddTransient<IProjectService, ProjectService>();
                services.AddTransient<IUserService, UserService>();
                services.AddTransient<IProductService, ProductService>();





                services.AddSingleton<MainWindow>();
                services.AddSingleton<MainViewModel>();

                services.AddTransient<CustomerListViewModel>();
                services.AddTransient<CustomerListView>();

                services.AddTransient<CustomerDetailViewModel>();
                services.AddTransient<CustomerDetailView>();

                services.AddTransient<CustomerNewViewModel>();
                services.AddTransient<CustomerNewView>();
                
                services.AddTransient<CustomerEditViewModel>();
                services.AddTransient<CustomerEditView>();

                services.AddTransient<ProjectListViewModel>();
                services.AddTransient<ProjectListView>();

                services.AddTransient<ProjectDetailViewModel>();
                services.AddTransient<ProjectDetailView>();
                
                services.AddTransient<ProjectNewViewModel>();
                services.AddTransient<ProjectNewView>();

                services.AddTransient<ProjectEditViewModel>();
                services.AddTransient<ProjectEditView>();
                
                services.AddTransient<UserListViewModel>();
                services.AddTransient<UserListView>();
                
                services.AddTransient<UserDetailViewModel>();
                services.AddTransient<UserDetailView>();
                
                services.AddTransient<UserNewViewModel>();
                services.AddTransient<UserNewView>();
                
                services.AddTransient<UserEditViewModel>();
                services.AddTransient<UserEditView>();
                
                services.AddTransient<ProductListViewModel>();
                services.AddTransient<ProductListView>();
                
                services.AddTransient<ProductDetailViewModel>();
                services.AddTransient<ProductDetailView>();
                
                services.AddTransient<ProductNewViewModel>();
                services.AddTransient<ProductNewView>();
                
                services.AddTransient<ProductEditViewModel>();
                services.AddTransient<ProductEditView>();
            })
            .Build();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

}
