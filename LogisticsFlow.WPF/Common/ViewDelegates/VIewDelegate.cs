using LogisticsFlow.WPF.Views;
using System.Windows;

namespace LogisticsFlow.WPF.Common.ViewDelegates
{
    public static class WindowDelegate
    {
        public static Func<Window?> VisibleAuthView = VisibleWindow<AuthView>();
        public static Func<Window?> VisiblePofileVeiw= VisibleWindow<ProfileView>();
        public static Func<Window?> VisibleMenuView = VisibleWindow<MenuView>();

        public static Func<Window?> VisibleOrderView = VisibleWindow<OrderView>();

        public static Func<Window?> VisibleCargoView = VisibleWindow<CargoView>();
        public static Func<Window?> VisibleClientView = VisibleWindow<ClientView>();
        public static Func<Window?> VisibleDriverView = VisibleWindow<DriverView>();
        public static Func<Window?> VisibleEmployeeView = VisibleWindow<EmployeeView>();
        public static Func<Window?> VisibleVehicleView = VisibleWindow<VehicleView>();
        public static Func<Window?> VisibleRouteView = VisibleWindow<RouteView>();

        private static Func<Window?> VisibleWindow<TWindow>() where TWindow : Window =>
            () => System.Windows
                .Application
                .Current
                .Windows
                .OfType<TWindow>()
                .FirstOrDefault(window => 
                    window.IsVisible);

        public static Func<List<Window>> VisibleWindows = () =>
            System.Windows
                .Application
                .Current
                .Windows
                .OfType<Window>()
                .ToList();
    }
}
