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
    public partial class RouteViewModel
        : ObservableObject,
        IBaseCRUD
    {
        [ObservableProperty]
        private Route _routeAdd = new Route();

        [ObservableProperty]
        private Route _routeWorker = new Route();

        [ObservableProperty]
        private ObservableCollection<Route> _routeList = new ObservableCollection<Route>();

        [ObservableProperty]
        private MessageVm _message = new MessageVm();

        private readonly LogisticsFlowDbContext _context;
        private readonly IViewManager _manager;

        public RouteViewModel(
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
                if (RouteAdd == null)
                {
                    Message.SetMassage(
                        MessageState.Warning,
                        MessageDefault.UINotFoundMessageDefault);
                    return;
                }

                using var tokenSource = new CancellationTokenSource(
                    Consts.TokenTimeoutMilliseconds);

                var entity = new Route()
                {
                    Id = Guid.NewGuid(),
                    Origin = RouteAdd.Origin,
                    Destination = RouteAdd.Destination,
                    Distance = RouteAdd.Distance,
                    EstimatedTime = RouteAdd.EstimatedTime
                };

                await _context.Routes.AddAsync(entity, tokenSource.Token);
                await _context.SaveChangesAsync(tokenSource.Token);

                RouteAdd = new Route();
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
                if (RouteWorker == null || RouteWorker.Id == Guid.Empty)
                {
                    Message.SetMassage(
                        MessageState.Warning,
                        MessageDefault.UINotFoundMessageDefault);
                    return;
                }

                using var tokenSource = new CancellationTokenSource(Consts.TokenTimeoutMilliseconds);

                var entity = await _context.Routes
                    .FirstOrDefaultAsync(e => e.Id == RouteWorker.Id, tokenSource.Token);

                if (entity == null)
                {
                    Message.SetMassage(
                        MessageState.Warning,
                        MessageDefault.NotFoundMessageDefault);
                    return;
                }

                entity.Origin = RouteWorker.Origin;
                entity.Destination = RouteWorker.Destination;
                entity.Distance = RouteWorker.Distance;
                entity.EstimatedTime = RouteWorker.EstimatedTime;

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
                if (RouteWorker == null || RouteWorker.Id == Guid.Empty)
                {
                    Message.SetMassage(
                        MessageState.Warning,
                        MessageDefault.UINotFoundMessageDefault);
                    return;
                }

                using var tokenSource = new CancellationTokenSource(Consts.TokenTimeoutMilliseconds);

                var entity = await _context.Routes
                    .FirstOrDefaultAsync(entity => 
                        entity.Id == RouteWorker.Id, 
                        tokenSource.Token);

                if (entity == null)
                {
                    Message.SetMassage(
                        MessageState.Warning,
                        MessageDefault.NotFoundMessageDefault);
                    return;
                }

                _context.Routes.Remove(entity);
                await _context.SaveChangesAsync(tokenSource.Token);

                RouteWorker = new Route();
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

                var entities = await _context.Routes
                    .AsNoTracking()
                    .ToListAsync(tokenSource.Token);

                RouteList = new ObservableCollection<Route>(entities);

                Message.SetMassage(
                    MessageState.Success,
                    $"{RouteList.Count}");
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
