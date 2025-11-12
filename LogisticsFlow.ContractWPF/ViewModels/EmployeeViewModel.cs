using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LogisticsFlow.ContractWPF.Common.Consts;
using LogisticsFlow.ContractWPF.Common.ContractModels.MessageVMs;
using LogisticsFlow.ContractWPF.Common.Interfaces.InterfacesWindowManagers;
using LogisticsFlow.Domain.Interfaces;
using LogisticsFlow.Domain.Models;
using LogisticsFlow.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace LogisticsFlow.ContractWPF.ViewModels
{
    public partial class EmployeeViewModel
        : ObservableObject,
        IBaseCRUD
    {
        [ObservableProperty]
        private Employee _employeeAdd = new Employee();

        [ObservableProperty]
        private Employee _employeeWorker = new Employee();

        [ObservableProperty]
        private ObservableCollection<Employee> _employeeList = new ObservableCollection<Employee>();

        [ObservableProperty]
        private MessageVm _message = new MessageVm();

        private readonly LogisticsFlowDbContext _context;
        private readonly IViewManager _manager;

        public EmployeeViewModel(
            LogisticsFlowDbContext context,
            IViewManager manager)
        {
            _context = context;
            _manager = manager;
        }

        [RelayCommand]
        public async Task CreateAsync()
        {
            try
            {
                if (EmployeeAdd == null)
                {
                    Message.SetMassage(
                        MessageState.Warning,
                        MessageDefault.UINotFoundMessageDefault);
                    return;
                }

                using var tokenSource = new CancellationTokenSource(
                    Consts.TokenTimeoutMilliseconds);

                var entity = new Employee()
                {
                    Id = Guid.NewGuid(),
                    Name = EmployeeAdd.Name,
                    Password = EmployeeAdd.Password,
                    Email = EmployeeAdd.Email,
                    PhoneNumber = EmployeeAdd.PhoneNumber
                };

                await _context.Employees.AddAsync(entity, tokenSource.Token);
                await _context.SaveChangesAsync(tokenSource.Token);

                EmployeeAdd = new Employee();
                await GetAllAsync();

                Message.SetMassage(
                    MessageState.Success,
                    MessageDefault.CreatedMessageDefault);
            }
            catch (OperationCanceledException)
            {
                Message.SetMassage(
                    MessageState.Error,
                    MessageDefault.TimeoutMessageDefault);
            }
            catch (Exception ex)
            {
                Message.SetMassage(
                   MessageState.Error,
                   $"{MessageDefault.ErrorMessageDefault} {ex.Message}");
            }
        }

        [RelayCommand]
        public async Task UpdateAsync()
        {
            try
            {
                if (EmployeeWorker == null || EmployeeWorker.Id == Guid.Empty)
                {
                    Message.SetMassage(
                        MessageState.Warning,
                        MessageDefault.UINotFoundMessageDefault);
                    return;
                }

                using var tokenSource = new CancellationTokenSource(Consts.TokenTimeoutMilliseconds);

                var entity = await _context.Employees
                    .FirstOrDefaultAsync(e => e.Id == EmployeeWorker.Id, tokenSource.Token);

                if (entity == null)
                {
                    Message.SetMassage(
                        MessageState.Warning,
                        MessageDefault.NotFoundMessageDefault);
                    return;
                }

                entity.Name = EmployeeWorker.Name;
                entity.Password = EmployeeWorker.Password;
                entity.Email = EmployeeWorker.Email;
                entity.PhoneNumber = EmployeeWorker.PhoneNumber;

                _context.Update(entity);
                await _context.SaveChangesAsync(tokenSource.Token);
                await GetAllAsync();

                Message.SetMassage(
                    MessageState.Success,
                    MessageDefault.UpdatedMessageDefault);
            }
            catch (OperationCanceledException)
            {
                Message.SetMassage(
                    MessageState.Error,
                    MessageDefault.TimeoutMessageDefault);
            }
            catch (Exception ex)
            {
                Message.SetMassage(
                   MessageState.Error,
                   $"{MessageDefault.ErrorMessageDefault} {ex.Message}");
            }
        }

        [RelayCommand]
        public async Task DeleteAsync()
        {
            try
            {
                if (EmployeeWorker == null || EmployeeWorker.Id == Guid.Empty)
                {
                    Message.SetMassage(
                        MessageState.Warning,
                        MessageDefault.UINotFoundMessageDefault);
                    return;
                }

                using var tokenSource = new CancellationTokenSource(Consts.TokenTimeoutMilliseconds);

                var entity = await _context.Employees
                    .FirstOrDefaultAsync(entity => 
                        entity.Id == EmployeeWorker.Id, 
                        tokenSource.Token);

                if (entity == null)
                {
                    Message.SetMassage(
                        MessageState.Warning,
                        MessageDefault.NotFoundMessageDefault);
                    return;
                }

                _context.Employees.Remove(entity);
                await _context.SaveChangesAsync(tokenSource.Token);

                EmployeeWorker = new Employee();
                await GetAllAsync();

                Message.SetMassage(
                    MessageState.Success,
                    MessageDefault.DeletedMessageDefault);
            }
            catch (OperationCanceledException)
            {
                Message.SetMassage(
                    MessageState.Error,
                    MessageDefault.TimeoutMessageDefault);
            }
            catch (Exception ex)
            {
                Message.SetMassage(
                   MessageState.Error,
                   $"{MessageDefault.ErrorMessageDefault} {ex.Message}");
            }
        }

        [RelayCommand]
        public async Task GetAllAsync()
        {
            try
            {
                using var tokenSource = new CancellationTokenSource(Consts.TokenTimeoutMilliseconds);

                var entities = await _context.Employees
                    .AsNoTracking()
                    .ToListAsync(tokenSource.Token);

                EmployeeList = new ObservableCollection<Employee>(entities);

                Message.SetMassage(
                    MessageState.Success,
                    $"{EmployeeList.Count}");
            }
            catch (OperationCanceledException)
            {
                Message.SetMassage(
                    MessageState.Error,
                    MessageDefault.TimeoutMessageDefault);
            }
            catch (Exception ex)
            {
                Message.SetMassage(
                   MessageState.Error,
                   $"{MessageDefault.ErrorMessageDefault} {ex.Message}");
            }
        }

        [RelayCommand]
        private void MenuView()
        {
            _manager.MenuWindow();
        }
    }
}
