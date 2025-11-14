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
using System.ComponentModel.DataAnnotations;

namespace LogisticsFlow.ContractWPF.ViewModels
{
    public partial class OrderViewModel
        : ObservableObject,
        IBaseCRUD
    {
        [ObservableProperty]
        private Order _orderAdd = new Order();

        [ObservableProperty]
        private Order _orderWorker = new Order();

        [ObservableProperty]
        private ObservableCollection<Order> _orderList = new ObservableCollection<Order>();

        [ObservableProperty]
        private ObservableCollection<Cargo> _cargoList = new ObservableCollection<Cargo>();

        [ObservableProperty]
        private ObservableCollection<Employee> _employeeList = new ObservableCollection<Employee>();

        [ObservableProperty]
        private ObservableCollection<Client> _clientList = new ObservableCollection<Client>();

        [ObservableProperty]
        private ObservableCollection<Driver> _driverList = new ObservableCollection<Driver>();

        [ObservableProperty]
        private ObservableCollection<Route> _routeList = new ObservableCollection<Route>();

        [ObservableProperty]
        private string _driverNameSearch = string.Empty;

        [ObservableProperty]
        private string _clientPhoneNumber = string.Empty;

        [ObservableProperty]
        private string _orderStatus = string.Empty;

        [ObservableProperty]
        private string _cargoName = string.Empty;



        [ObservableProperty]
        private MessageVm _message = new MessageVm();

        private readonly LogisticsFlowDbContext _context;
        private readonly IViewManager _manager;

        public OrderViewModel(
            LogisticsFlowDbContext context,
            IViewManager manager)
        {
            _context = context;
            _manager = manager;
        }

        [RelayCommand]
        public async Task SearchAsync()
        {
            try
            {
                using var tokenSource = new CancellationTokenSource();

                var entities = await _context.Orders
                    .AsNoTracking()
                    .Include(order => order.Cargo)
                    .Include(order => order.Employee)
                    .Include(order => order.Client)
                    .Include(order => order.Driver)
                    .Include(order => order.Route)
                    .Where(order =>
                        (string.IsNullOrWhiteSpace(DriverNameSearch)
                         || order.Driver.Name.Contains(DriverNameSearch))
                        && (string.IsNullOrWhiteSpace(ClientPhoneNumber)
                         || order.Client.PhoneNumber.Contains(ClientPhoneNumber))
                        && (string.IsNullOrWhiteSpace(OrderStatus)
                         || order.Status.Contains(OrderStatus))
                        && (string.IsNullOrWhiteSpace(CargoName)
                         || order.Cargo.Name.Contains(CargoName))
                    )
                    .ToListAsync(tokenSource.Token);

                OrderList = new ObservableCollection<Order>(entities);

                Message.SetMassage(
                    MessageState.Success,
                    $"Найдено записей: {OrderList.Count}");
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
        public async Task CreateAsync()
        {
            try
            {
                if (OrderAdd == null)
                {
                    Message.SetMassage(
                        MessageState.Warning,
                        MessageDefault.UINotFoundMessageDefault);
                    return;
                }

                using var tokenSource = new CancellationTokenSource(
                    Consts.TokenTimeoutMilliseconds);

                var entity = new Order
                {
                    Id = Guid.NewGuid(),
                    OrderDate = DateTime.Now,
                    DesiredDeliveryDate = OrderAdd.DesiredDeliveryDate,
                    Status = OrderAdd.Status,
                    Price = OrderAdd.Price,
                    CargoId = OrderAdd.CargoId,
                    EmployeeId = OrderAdd.EmployeeId,
                    ClientId = OrderAdd.ClientId,
                    DriverId = OrderAdd.DriverId,
                    RouteId = OrderAdd.RouteId
                };

                await _context.Orders.AddAsync(entity, tokenSource.Token);
                await _context.SaveChangesAsync(tokenSource.Token);

                OrderAdd = new Order();
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
                if (OrderWorker == null || OrderWorker.Id == Guid.Empty)
                {
                    Message.SetMassage(
                        MessageState.Warning,
                        MessageDefault.UINotFoundMessageDefault);
                    return;
                }

                using var tokenSource = new CancellationTokenSource(Consts.TokenTimeoutMilliseconds);

                var entity = await _context.Orders
                    .FirstOrDefaultAsync(e => e.Id == OrderWorker.Id, tokenSource.Token);

                if (entity == null)
                {
                    Message.SetMassage(
                        MessageState.Warning,
                        MessageDefault.NotFoundMessageDefault);
                    return;
                }

                entity.OrderDate = OrderWorker.OrderDate;
                entity.DesiredDeliveryDate = OrderWorker.DesiredDeliveryDate;
                entity.Status = OrderWorker.Status;
                entity.Price = OrderWorker.Price;
                entity.CargoId = OrderWorker.CargoId;
                entity.EmployeeId = OrderWorker.EmployeeId;
                entity.ClientId = OrderWorker.ClientId;
                entity.DriverId = OrderWorker.DriverId;
                entity.RouteId = OrderWorker.RouteId;

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
                if (OrderWorker == null || OrderWorker.Id == Guid.Empty)
                {
                    Message.SetMassage(
                        MessageState.Warning,
                        MessageDefault.UINotFoundMessageDefault);
                    return;
                }

                using var tokenSource = new CancellationTokenSource(Consts.TokenTimeoutMilliseconds);

                var entity = await _context.Orders
                    .FirstOrDefaultAsync(entity =>
                        entity.Id == OrderWorker.Id,
                        tokenSource.Token);

                if (entity == null)
                {
                    Message.SetMassage(
                        MessageState.Warning,
                        MessageDefault.NotFoundMessageDefault);
                    return;
                }

                _context.Orders.Remove(entity);
                await _context.SaveChangesAsync(tokenSource.Token);

                OrderWorker = new Order();
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

                var entities = await _context.Orders
                    .Include(order => order.Cargo)
                    .Include(order => order.Employee)
                    .Include(order => order.Client)
                    .Include(order => order.Driver)
                    .Include(order => order.Route)
                    .AsNoTracking()
                    .ToListAsync(tokenSource.Token);

                var cargos = await _context.Cargos.AsNoTracking().ToListAsync(tokenSource.Token);
                var employees = await _context.Employees.AsNoTracking().ToListAsync(tokenSource.Token);
                var clients = await _context.Clients.AsNoTracking().ToListAsync(tokenSource.Token);
                var drivers = await _context.Drivers.AsNoTracking().ToListAsync(tokenSource.Token);
                var routes = await _context.Routes.AsNoTracking().ToListAsync(tokenSource.Token);

                OrderList = new ObservableCollection<Order>(entities);
                CargoList = new ObservableCollection<Cargo>(cargos);
                EmployeeList = new ObservableCollection<Employee>(employees);
                ClientList = new ObservableCollection<Client>(clients);
                DriverList = new ObservableCollection<Driver>(drivers);
                RouteList = new ObservableCollection<Route>(routes);

                Message.SetMassage(
                    MessageState.Success,
                    $"{OrderList.Count}");
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
