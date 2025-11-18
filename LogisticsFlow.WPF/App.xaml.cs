using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using LogisticsFlow.ContractWPF.Common.Interfaces.InterfacesWindowManagers;
using LogisticsFlow.WPF.Common.ViewManagers;
using LogisticsFlow.ContractWPF.DependencyInjections;
using LogisticsFlow.WPF.Views;
using LogisticsFlow.ContractWPF.ViewModels;

namespace LogisticsFlow.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider? _serviceProvider;
        private IViewManager? _windowManager;

        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();

            ConfigureServices(services);

            _serviceProvider = services.BuildServiceProvider();

            _windowManager = new ViewManager(_serviceProvider);
            _windowManager.AuthView();

            base.OnStartup(e);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Menu
            services.AddTransient<MenuView>();
            services.AddTransient<MenuViewModel>();

            services.AddTransient<ProfileView>();
            services.AddTransient<ProfileViewModel>();
            // Auth
            services.AddTransient<AuthView>();
            services.AddTransient<AuthViewModel>();

            // Order
            services.AddTransient<OrderView>();
            services.AddTransient<OrderViewModel>();

            // Cargo
            services.AddTransient<CargoView>();
            services.AddTransient<CargoViewModel>();

            // Client
            services.AddTransient<ClientView>();
            services.AddTransient<ClientViewModel>();

            // Driver
            services.AddTransient<DriverView>();
            services.AddTransient<DriverViewModel>();

            // Employee
            services.AddTransient<EmployeeView>();
            services.AddTransient<EmployeeViewModel>();

            // Vehicle
            services.AddTransient<VehicleView>();
            services.AddTransient<VehicleViewModel>();

            // Route
            services.AddTransient<RouteView>();
            services.AddTransient<RouteViewModel>();

            // ViewManager
            services.AddSingleton<IViewManager, ViewManager>();

            // Contract WPF services
            services.AddContractWPF();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (_serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }

            base.OnExit(e);
        }
    }
}
