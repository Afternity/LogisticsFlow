using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LogisticsFlow.ContractWPF.Common.ContractModels.MessageVMs;
using LogisticsFlow.ContractWPF.Common.ContractModels.UserProfile;
using LogisticsFlow.ContractWPF.Common.Interfaces.InterfacesWindowManagers;
using LogisticsFlow.Domain.Models;

namespace LogisticsFlow.ContractWPF.ViewModels
{
    public partial class ProfileViewModel
     : ObservableObject
    {
        [ObservableProperty]
        private Employee _employee = new Employee();

        [ObservableProperty]
        private MessageVm _message = new MessageVm();

        private readonly IViewManager _viewManager;

        public ProfileViewModel(IViewManager viewManager)
        {
            _viewManager = viewManager;

            // Загружаем данные сотрудника из статического класса
            if (UserProfileVm.ProvileVm != null)
            {
                Employee = UserProfileVm.ProvileVm;
                Message.SetMassage(MessageState.Success, "Профиль успешно загружен");
            }
            else
            {
                Message.SetMassage(MessageState.Warning, "Данные профиля не найдены");
            }
        }

        [RelayCommand]
        private void MenuView()
        {
            _viewManager.MenuWindow();
        }
    }
}
