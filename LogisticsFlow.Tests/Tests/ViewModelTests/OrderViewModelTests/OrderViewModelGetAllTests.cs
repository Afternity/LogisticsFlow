using LogisticsFlow.ContractWPF.Common.ContractModels.MessageVMs;
using LogisticsFlow.ContractWPF.Common.Interfaces.InterfacesWindowManagers;
using LogisticsFlow.ContractWPF.ViewModels;
using LogisticsFlow.Domain.Models;
using LogisticsFlow.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace LogisticsFlow.Tests.Tests.ViewModelTests.OrderViewModelTests
{
    public class OrderViewModelGetAllTests
        : IDisposable
    {
        private LogisticsFlowDbContext _context;

        private LogisticsFlowDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<LogisticsFlowDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new LogisticsFlowDbContext(options);
        }

        [Fact]
        public async Task GetAllAsync_WithOrders_ReturnsAllOrdersAndRelatedData()
        {
            // Arrange
            _context = CreateInMemoryContext();

            // Создаем тестовые данные
            var cargo = new Cargo { Id = Guid.NewGuid(), Name = "Груз 1" };
            var employee = new Employee { Id = Guid.NewGuid(), Name = "Сотрудник 1" };
            var client = new Client { Id = Guid.NewGuid(), PhoneNumber = "+79123456789" };
            var driver = new Driver { Id = Guid.NewGuid(), Name = "Водитель 1" };
            var route = new Route { Id = Guid.NewGuid(), Origin = "A", Destination = "B" };

            await _context.Cargos.AddAsync(cargo);
            await _context.Employees.AddAsync(employee);
            await _context.Clients.AddAsync(client);
            await _context.Drivers.AddAsync(driver);
            await _context.Routes.AddAsync(route);
            await _context.SaveChangesAsync();

            var orders = new List<Order>
            {
                new Order
                {
                    Id = Guid.NewGuid(),
                    Status = "В процессе",
                    Price = 1000,
                    CargoId = cargo.Id,
                    EmployeeId = employee.Id,
                    ClientId = client.Id,
                    DriverId = driver.Id,
                    RouteId = route.Id
                },
                new Order
                {
                    Id = Guid.NewGuid(),
                    Status = "Завершен",
                    Price = 2000,
                    CargoId = cargo.Id,
                    EmployeeId = employee.Id,
                    ClientId = client.Id,
                    DriverId = driver.Id,
                    RouteId = route.Id
                }
            };

            await _context.Orders.AddRangeAsync(orders);
            await _context.SaveChangesAsync();

            var mockViewManager = new Mock<IViewManager>();
            var viewModel = new OrderViewModel(_context, mockViewManager.Object);

            // Act
            await viewModel.GetAllAsync();

            // Assert
            Assert.Equal(2, viewModel.OrderList.Count);
            Assert.Single(viewModel.CargoList);
            Assert.Single(viewModel.EmployeeList);
            Assert.Single(viewModel.ClientList);
            Assert.Single(viewModel.DriverList);
            Assert.Single(viewModel.RouteList);
            Assert.Equal(MessageState.Success, viewModel.Message.State);
        }

        [Fact]
        public async Task GetAllAsync_EmptyDatabase_ReturnsEmptyLists()
        {
            // Arrange
            _context = CreateInMemoryContext(); // Пустая база
            var mockViewManager = new Mock<IViewManager>();
            var viewModel = new OrderViewModel(_context, mockViewManager.Object);

            // Act
            await viewModel.GetAllAsync();

            // Assert
            Assert.Empty(viewModel.OrderList);
            Assert.Empty(viewModel.CargoList);
            Assert.Empty(viewModel.EmployeeList);
            Assert.Empty(viewModel.ClientList);
            Assert.Empty(viewModel.DriverList);
            Assert.Empty(viewModel.RouteList);
            Assert.Equal(MessageState.Success, viewModel.Message.State);
        }

        [Fact]
        public async Task GetAllAsync_OrdersWithRelatedData_LoadsNavigationProperties()
        {
            // Arrange
            _context = CreateInMemoryContext();

            // Создаем ВСЕ обязательные связанные сущности
            var cargo = new Cargo { Id = Guid.NewGuid(), Name = "Тестовый груз" };
            var employee = new Employee { Id = Guid.NewGuid(), Name = "Тестовый сотрудник" };
            var client = new Client { Id = Guid.NewGuid(), PhoneNumber = "+79123456789" };
            var driver = new Driver { Id = Guid.NewGuid(), Name = "Тестовый водитель" };
            var route = new Route { Id = Guid.NewGuid(), Origin = "A", Destination = "B" };

            await _context.Cargos.AddAsync(cargo);
            await _context.Employees.AddAsync(employee);
            await _context.Clients.AddAsync(client);
            await _context.Drivers.AddAsync(driver);
            await _context.Routes.AddAsync(route);
            await _context.SaveChangesAsync();

            var order = new Order
            {
                Id = Guid.NewGuid(),
                Status = "В процессе",
                Price = 1500,
                CargoId = cargo.Id,
                EmployeeId = employee.Id,
                ClientId = client.Id,
                DriverId = driver.Id,
                RouteId = route.Id
            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            var mockViewManager = new Mock<IViewManager>();
            var viewModel = new OrderViewModel(_context, mockViewManager.Object);

            // Act
            await viewModel.GetAllAsync();

            // Assert
            Assert.Single(viewModel.OrderList); // Проверяем что есть один заказ

            var loadedOrder = viewModel.OrderList.FirstOrDefault(); // Используем FirstOrDefault вместо First
            Assert.NotNull(loadedOrder); // Проверяем что заказ не null

            // Проверяем что навигационные свойства загружены
            Assert.NotNull(loadedOrder.Cargo);
            Assert.NotNull(loadedOrder.Driver);
            Assert.Equal("Тестовый груз", loadedOrder.Cargo.Name);
            Assert.Equal("Тестовый водитель", loadedOrder.Driver.Name);
            Assert.Equal(MessageState.Success, viewModel.Message.State);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
