using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LogisticsFlow.ContractWPF.Common.Consts;
using LogisticsFlow.ContractWPF.Common.ContractModels.MessageVMs;
using LogisticsFlow.ContractWPF.Common.Interfaces.InterfacesWindowManagers;
using LogisticsFlow.Domain.Interfaces;
using LogisticsFlow.Domain.Models;
using LogisticsFlow.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Collections.ObjectModel;

namespace LogisticsFlow.ContractWPF.ViewModels
{
    public partial class CargoViewModel
        : ObservableObject,
        IBaseCRUD
    {
        [ObservableProperty]
        private Cargo _cargoAdd = new Cargo();

        [ObservableProperty]
        private Cargo _cargoWorker = new Cargo();

        [ObservableProperty]
        private ObservableCollection<Cargo> _cargoList = new ObservableCollection<Cargo>();

        [ObservableProperty]
        private MessageVm _message = new MessageVm();

        private readonly LogisticsFlowDbContext _context;
        private readonly IViewManager _manager;

        public CargoViewModel(
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
                if (CargoAdd == null)
                {
                    Message.SetMassage(
                        MessageState.Warning,
                        MessageDefault.UINotFoundMessageDefault);
                    return;
                }

                using var tokenSource = new CancellationTokenSource(
                    Consts.TokenTimeoutMilliseconds);

                var entity = new Cargo()
                {
                    Id = Guid.NewGuid(),
                    Name = CargoAdd.Name,
                    Description = CargoAdd.Description,
                    Weight = CargoAdd.Weight,
                    Volume = CargoAdd.Volume
                };

                await _context.Cargos.AddAsync(entity, tokenSource.Token);
                await _context.SaveChangesAsync(tokenSource.Token);

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
                if (CargoWorker == null || CargoWorker.Id == Guid.Empty)
                {
                    Message.SetMassage(
                        MessageState.Warning,
                        MessageDefault.UINotFoundMessageDefault);
                    return;
                }

                using var tokenSource = new CancellationTokenSource(Consts.TokenTimeoutMilliseconds);

                var entity = await _context.Cargos
                    .FirstOrDefaultAsync(e => e.Id == CargoWorker.Id, tokenSource.Token);

                if (entity == null)
                {
                    Message.SetMassage(
                        MessageState.Warning,
                        MessageDefault.NotFoundMessageDefault);
                    return;
                }

                entity.Name = CargoWorker.Name;
                entity.Description = CargoWorker.Description;
                entity.Weight = CargoWorker.Weight;
                entity.Volume = CargoWorker.Volume;

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
                if (CargoWorker == null || CargoWorker.Id == Guid.Empty)
                {
                    Message.SetMassage(
                        MessageState.Warning,
                        MessageDefault.UINotFoundMessageDefault);
                    return;
                }

                using var tokenSource = new CancellationTokenSource(Consts.TokenTimeoutMilliseconds);

                var entity = await _context.Cargos
                    .FirstOrDefaultAsync(entity => 
                        entity.Id == CargoWorker.Id, 
                        tokenSource.Token);

                if (entity == null)
                {
                    Message.SetMassage(
                        MessageState.Warning,
                        MessageDefault.NotFoundMessageDefault);
                    return;
                }

                _context.Cargos.Remove(entity);
                await _context.SaveChangesAsync(tokenSource.Token);
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

                var entities = await _context.Cargos
                    .AsNoTracking()
                    .ToListAsync(tokenSource.Token);

                CargoList = new ObservableCollection<Cargo>(entities);

                Message.SetMassage(
                    MessageState.Success,
                    $"{CargoList.Count}");
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
