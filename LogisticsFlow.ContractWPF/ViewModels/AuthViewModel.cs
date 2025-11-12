using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LogisticsFlow.ContractWPF.Common.Consts;
using LogisticsFlow.ContractWPF.Common.ContractModels.MessageVMs;
using LogisticsFlow.ContractWPF.Common.ContractModels.UserProfile;
using LogisticsFlow.ContractWPF.Common.Interfaces.InterfacesWindowManagers;
using LogisticsFlow.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;


namespace LogisticsFlow.ContractWPF.ViewModels
{
    public partial class AuthViewModel
        : ObservableObject
    {
        [ObservableProperty]
        private string _employeeLogin = "1";

        [ObservableProperty]
        private string _employeePassword = "1";

        [ObservableProperty]
        private MessageVm _messageVm = new MessageVm();

        private readonly LogisticsFlowDbContext _context;
        private readonly IViewManager _viewManager;

        public AuthViewModel(
            LogisticsFlowDbContext context,
            IViewManager windowManager)
        {
            _context = context;
            _viewManager = windowManager;
        }

        [RelayCommand]
        private async Task Auth()
        {
            try
            {
                using var tokenSource = new CancellationTokenSource(
                   Consts.TokenTimeoutMilliseconds);

                var authPersonal = await _context.Employees
                    .FirstOrDefaultAsync(authPersonal =>
                        authPersonal.Email == EmployeeLogin
                        && authPersonal.Password == EmployeePassword,
                        tokenSource.Token);

                if (authPersonal == null)
                {
                    MessageVm.SetMassage(
                        MessageState.Fail,
                        "Authentication failed.");
                    return;
                }

                UserProfileVm.ProvileVm = authPersonal;

                MessageVm.SetMassage(
                    MessageState.Success,
                    "Authentication");

                await Task.Delay(2000);

                MenuView();
            }
            catch (OperationCanceledException)
            {
                MessageVm.SetMassage(
                    MessageState.Fail,
                    MessageDefault.TimeoutMessageDefault);
            }
            catch (Exception ex)
            {
                MessageVm.SetMassage(
                    MessageState.Error,
                    $"{MessageDefault.ErrorMessageDefault} {ex}");
            }
        }

        [RelayCommand]
        public void MenuView()
        {
            _viewManager.MenuWindow();
        }
    }
}