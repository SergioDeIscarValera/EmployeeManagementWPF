using EmployeeManagementWPF.MVVM.ViewModel;
using EmployeeManagementWPF.Properties;
using EmployeeManagementWPF.Service.Repository.DepartmentRepo;
using EmployeeManagementWPF.Service.Repository.EmployeeRepo;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace EmployeeManagementWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public new static App Current => (App)Application.Current;
        public IServiceProvider Services { get; }
        public App()
        {
            var lang = Settings.Default.current_languaje;
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(lang);

            Services = ConfigureServices();
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Services
            services.AddSingleton<IDepartmentRepository, DepartmentRepositoryMongo>();
            services.AddSingleton<IEmployeeRepository, EmployeeRepositoryMongo>();

            // ViewModels
            services.AddTransient<HomeViewModel>();
            services.AddTransient<DepartmentViewModel>();
            services.AddTransient<EmployeeViewModel>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<SettingsViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
