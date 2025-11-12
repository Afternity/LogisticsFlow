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
    public partial class VehicleViewModel
        : ObservableObject,
        IBaseCRUD
    {
        [ObservableProperty]
        private Vehicle _vehicleAdd = new Vehicle();

        [ObservableProperty]
        private Vehicle _vehicleWorker = new Vehicle();

        [ObservableProperty]
        private ObservableCollection<Vehicle> _vehicleList = new ObservableCollection<Vehicle>();

        [ObservableProperty]
        private MessageVm _message = new MessageVm();

        private readonly LogisticsFlowDbContext _context;
        private readonly IViewManager _manager;

        public VehicleViewModel(
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
                if (VehicleAdd == null)
                {
                    Message.SetMassage(
                        MessageState.Warning,
                        MessageDefault.UINotFoundMessageDefault);
                    return;
                }

                using var tokenSource = new CancellationTokenSource(
                    Consts.TokenTimeoutMilliseconds);

                var entity = new Vehicle()
                {
                    Id = Guid.NewGuid(),
                    Mark = VehicleAdd.Mark,
                    Model = VehicleAdd.Model,
                    Number = VehicleAdd.Number,
                    LoadCapacity = VehicleAdd.LoadCapacity
                };

                await _context.Vehicles.AddAsync(entity, tokenSource.Token);
                await _context.SaveChangesAsync(tokenSource.Token);

                VehicleAdd = new Vehicle();
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
                if (VehicleWorker == null || VehicleWorker.Id == Guid.Empty)
                {
                    Message.SetMassage(
                        MessageState.Warning,
                        MessageDefault.UINotFoundMessageDefault);
                    return;
                }

                using var tokenSource = new CancellationTokenSource(Consts.TokenTimeoutMilliseconds);

                var entity = await _context.Vehicles
                    .FirstOrDefaultAsync(e => e.Id == VehicleWorker.Id, tokenSource.Token);

                if (entity == null)
                {
                    Message.SetMassage(
                        MessageState.Warning,
                        MessageDefault.NotFoundMessageDefault);
                    return;
                }

                entity.Mark = VehicleWorker.Mark;
                entity.Model = VehicleWorker.Model;
                entity.Number = VehicleWorker.Number;
                entity.LoadCapacity = VehicleWorker.LoadCapacity;

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
                if (VehicleWorker == null || VehicleWorker.Id == Guid.Empty)
                {
                    Message.SetMassage(
                        MessageState.Warning,
                        MessageDefault.UINotFoundMessageDefault);
                    return;
                }

                using var tokenSource = new CancellationTokenSource(Consts.TokenTimeoutMilliseconds);

                var entity = await _context.Vehicles
                    .FirstOrDefaultAsync(entity => 
                        entity.Id == VehicleWorker.Id, 
                        tokenSource.Token);

                if (entity == null)
                {
                    Message.SetMassage(
                        MessageState.Warning,
                        MessageDefault.NotFoundMessageDefault);
                    return;
                }

                _context.Vehicles.Remove(entity);
                await _context.SaveChangesAsync(tokenSource.Token);

                VehicleWorker = new Vehicle();
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

                var entities = await _context.Vehicles
                    .AsNoTracking()
                    .ToListAsync(tokenSource.Token);

                VehicleList = new ObservableCollection<Vehicle>(entities);

                Message.SetMassage(
                    MessageState.Success,
                    $"{VehicleList.Count}");
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
