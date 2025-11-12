using LogisticsFlow.ContractWPF.Common.Interfaces.InterfacesWindowManagers;
using LogisticsFlow.ContractWPF.ViewModels;
using LogisticsFlow.WPF.Common.ViewDelegates;
using LogisticsFlow.WPF.Views;
using Microsoft.Extensions.DependencyInjection;

namespace LogisticsFlow.WPF.Common.ViewManagers
{
    public class ViewManager
        : IViewManager
    {
        private readonly IServiceProvider _serviceProvider;

        public ViewManager(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void AuthView()
        {
            var auth = _serviceProvider.GetRequiredService<AuthView>();
            auth.DataContext = _serviceProvider.GetRequiredService<AuthViewModel>();
            auth.Show();

            var all = WindowDelegate.VisibleWindows?.Invoke();

            if (all == null)
                return;

            foreach (var w in all)
            {
                if (WindowDelegate.VisibleAuthView?.Invoke() != w)
                    w.Close();
            }
        }

        public void CargoView()
        {
            var oldView = WindowDelegate.VisibleCargoView?.Invoke();
            if (oldView != null)
            {
                oldView.Activate();
                return;
            }

            var newView = _serviceProvider.GetRequiredService<CargoView>();
            newView.DataContext = _serviceProvider.GetRequiredService<CargoViewModel>();
            newView.Show();
        }

        public void ClientView()
        {
            var oldView = WindowDelegate.VisibleClientView?.Invoke();
            if (oldView != null)
            {
                oldView.Activate();
                return;
            }

            var newView = _serviceProvider.GetRequiredService<ClientView>();
            newView.DataContext = _serviceProvider.GetRequiredService<ClientViewModel>();
            newView.Show();
        }

        public void DriverView()
        {
            var oldView = WindowDelegate.VisibleDriverView?.Invoke();
            if (oldView != null)
            {
                oldView.Activate();
                return;
            }

            var newView = _serviceProvider.GetRequiredService<DriverView>();
            newView.DataContext = _serviceProvider.GetRequiredService<DriverViewModel>();
            newView.Show();
        }

        public void EmployeeView()
        {
            var oldView = WindowDelegate.VisibleEmployeeView?.Invoke();
            if (oldView != null)
            {
                oldView.Activate();
                return;
            }

            var newView = _serviceProvider.GetRequiredService<EmployeeView>();
            newView.DataContext = _serviceProvider.GetRequiredService<EmployeeViewModel>();
            newView.Show();
        }

        public void MenuWindow()
        {
            var existing = WindowDelegate.VisibleMenuView?.Invoke();
            if (existing != null)
            {
                existing.Activate();
                return;
            }

            var order = _serviceProvider.GetRequiredService<MenuView>();
            order.DataContext = _serviceProvider.GetRequiredService<MenuViewModel>();
            order.Show();

            WindowDelegate.VisibleAuthView?.Invoke()?.Close();
        }

        public void OrderView()
        {
            var existing = WindowDelegate.VisibleOrderView?.Invoke();
            if (existing != null)
            {
                existing.Activate();
                return;
            }

            var order = _serviceProvider.GetRequiredService<OrderView>();
            order.DataContext = _serviceProvider.GetRequiredService<OrderViewModel>();
            order.Show();
        }

        public void ProfileView()
        {
            var oldView = WindowDelegate.VisiblePofileVeiw?.Invoke();
            if (oldView != null)
            {
                oldView.Activate();
                return;
            }

            var newView = _serviceProvider.GetRequiredService<ProfileView>();
            newView.DataContext = _serviceProvider.GetRequiredService<ProfileViewModel>();
            newView.Show();
        }

        public void RouteView()
        {
            var oldView = WindowDelegate.VisibleRouteView?.Invoke();
            if (oldView != null)
            {
                oldView.Activate();
                return;
            }

            var newView = _serviceProvider.GetRequiredService<RouteView>();
            newView.DataContext = _serviceProvider.GetRequiredService<RouteViewModel>();
            newView.Show();
        }

        public void VehicleView()
        {
            var oldView = WindowDelegate.VisibleVehicleView?.Invoke();
            if (oldView != null)
            {
                oldView.Activate();
                return;
            }

            var newView = _serviceProvider.GetRequiredService<VehicleView>();
            newView.DataContext = _serviceProvider.GetRequiredService<VehicleViewModel>();
            newView.Show();
        }
    }
}
