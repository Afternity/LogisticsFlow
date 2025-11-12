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
    public partial class ClientViewModel
        : ObservableObject,
        IBaseCRUD
    {
        [ObservableProperty]
        private Client _clientAdd = new Client();

        [ObservableProperty]
        private Client _clientWorker = new Client();

        [ObservableProperty]
        private ObservableCollection<Client> _clientList = new ObservableCollection<Client>();

        [ObservableProperty]
        private MessageVm _message = new MessageVm();

        private readonly LogisticsFlowDbContext _context;
        private readonly IViewManager _manager;

        public ClientViewModel(
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
                if (ClientAdd == null)
                {
                    Message.SetMassage(
                        MessageState.Warning,
                        MessageDefault.UINotFoundMessageDefault);
                    return;
                }

                using var tokenSource = new CancellationTokenSource(
                    Consts.TokenTimeoutMilliseconds);

                var entity = new Client()
                {
                    Id = Guid.NewGuid(),
                    Name = ClientAdd.Name,
                    Email = ClientAdd.Email,
                    PhoneNumber = ClientAdd.PhoneNumber,
                    Address = ClientAdd.Address
                };

                await _context.Clients.AddAsync(entity, tokenSource.Token);
                await _context.SaveChangesAsync(tokenSource.Token);

                ClientAdd = new Client();
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
                if (ClientWorker == null || ClientWorker.Id == Guid.Empty)
                {
                    Message.SetMassage(
                        MessageState.Warning,
                        MessageDefault.UINotFoundMessageDefault);
                    return;
                }

                using var tokenSource = new CancellationTokenSource(Consts.TokenTimeoutMilliseconds);

                var entity = await _context.Clients
                    .FirstOrDefaultAsync(e => e.Id == ClientWorker.Id, tokenSource.Token);

                if (entity == null)
                {
                    Message.SetMassage(
                        MessageState.Warning,
                        MessageDefault.NotFoundMessageDefault);
                    return;
                }

                entity.Name = ClientWorker.Name;
                entity.Email = ClientWorker.Email;
                entity.PhoneNumber = ClientWorker.PhoneNumber;
                entity.Address = ClientWorker.Address;

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
                if (ClientWorker == null || ClientWorker.Id == Guid.Empty)
                {
                    Message.SetMassage(
                        MessageState.Warning,
                        MessageDefault.UINotFoundMessageDefault);
                    return;
                }

                using var tokenSource = new CancellationTokenSource(Consts.TokenTimeoutMilliseconds);

                var entity = await _context.Clients
                    .FirstOrDefaultAsync(entity => 
                        entity.Id == ClientWorker.Id, 
                        tokenSource.Token);

                if (entity == null)
                {
                    Message.SetMassage(
                        MessageState.Warning,
                        MessageDefault.NotFoundMessageDefault);
                    return;
                }

                _context.Clients.Remove(entity);
                await _context.SaveChangesAsync(tokenSource.Token);

                ClientWorker = new Client();
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

                var entities = await _context.Clients
                    .AsNoTracking()
                    .ToListAsync(tokenSource.Token);

                ClientList = new ObservableCollection<Client>(entities);

                Message.SetMassage(
                    MessageState.Success,
                    $"{ClientList.Count}");
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
