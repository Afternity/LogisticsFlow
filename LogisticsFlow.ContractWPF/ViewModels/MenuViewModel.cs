using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LogisticsFlow.ContractWPF.Common.Interfaces.InterfacesWindowManagers;

namespace LogisticsFlow.ContractWPF.ViewModels
{
    public partial class MenuViewModel
       : ObservableObject,
        IViewManager
    {
        private readonly IViewManager _manager;

        public MenuViewModel(
            IViewManager manager)
        {
            _manager = manager;
        }

        [RelayCommand]
        public void AuthView()
        {
            _manager.AuthView();
        }

        [RelayCommand]
        public void CargoView()
        {
            _manager.CargoView();
        }

        [RelayCommand]
        public void ClientView()
        {
            _manager.ClientView();
        }

        [RelayCommand]
        public void DriverView()
        {
            _manager.DriverView();
        }

        [RelayCommand]
        public void EmployeeView()
        {
            _manager.EmployeeView();
        }

        [RelayCommand]
        public void MenuWindow()
        {
            _manager.MenuWindow();
        }

        [RelayCommand]
        public void OrderView()
        {
            _manager.OrderView();
        }

        [RelayCommand]
        public void ProfileView()
        {
            _manager.ProfileView();
        }

        [RelayCommand]
        public void RouteView()
        {
            _manager.RouteView();
        }

        [RelayCommand]
        public void VehicleView()
        {
            _manager.VehicleView();
        }
    }       
}
