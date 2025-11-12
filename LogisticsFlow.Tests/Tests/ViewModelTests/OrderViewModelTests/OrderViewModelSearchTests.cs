using LogisticsFlow.ContractWPF.Common.ContractModels.MessageVMs;
using LogisticsFlow.ContractWPF.Common.Interfaces.InterfacesWindowManagers;
using LogisticsFlow.ContractWPF.ViewModels;
using LogisticsFlow.Domain.Models;
using LogisticsFlow.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Moq;


namespace LogisticsFlow.Tests.Tests.ViewModelTests.OrderViewModelTests
{
    public class OrderViewModelSearchTests
       : IDisposable
    {
        private LogisticsFlowDbContext _context;

        private LogisticsFlowDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<LogisticsFlowDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .Options;

            return new LogisticsFlowDbContext(options);
        }

        [Fact]
        public async Task SearchAsync_NoFilters_ReturnsAllOrders()
        {
            // Arrange
            _context = CreateInMemoryContext();

            // ВАЖНО: Сначала сохраняем связанные сущности
            var driver = new Driver { Id = Guid.NewGuid(), Name = "Тестовый Водитель" };
            var client = new Client { Id = Guid.NewGuid(), PhoneNumber = "+79123456789" };
            var cargo = new Cargo { Id = Guid.NewGuid(), Name = "Тестовый Груз" };
            var employee = new Employee { Id = Guid.NewGuid(), Name = "Тестовый Сотрудник" };
            var route = new Route { Id = Guid.NewGuid(), Origin = "A", Destination = "B" };

            await _context.Drivers.AddAsync(driver);
            await _context.Clients.AddAsync(client);
            await _context.Cargos.AddAsync(cargo);
            await _context.Employees.AddAsync(employee);
            await _context.Routes.AddAsync(route);
            await _context.SaveChangesAsync();

            var orders = new List<Order>
            {
                new Order
                {
                    Id = Guid.NewGuid(),
                    Status = "В процессе",
                    Price = 1000,
                    DriverId = driver.Id,
                    ClientId = client.Id,
                    CargoId = cargo.Id,
                    EmployeeId = employee.Id,
                    RouteId = route.Id
                },
                new Order
                {
                    Id = Guid.NewGuid(),
                    Status = "Завершен",
                    Price = 2000,
                    DriverId = driver.Id,
                    ClientId = client.Id,
                    CargoId = cargo.Id,
                    EmployeeId = employee.Id,
                    RouteId = route.Id
                }
            };

            await _context.Orders.AddRangeAsync(orders);
            await _context.SaveChangesAsync();

            var mockViewManager = new Mock<IViewManager>();
            var viewModel = new OrderViewModel(_context, mockViewManager.Object);

            // Act
            await viewModel.SearchAsync();

            // Assert
            Assert.Equal(2, viewModel.OrderList.Count);
            Assert.Equal(MessageState.Success, viewModel.Message.State);
        }

        [Fact]
        public async Task SearchAsync_ByDriverName_ReturnsFilteredOrders()
        {
            // Arrange
            _context = CreateInMemoryContext();

            // Сначала создаем и сохраняем водителей
            var driver1 = new Driver { Id = Guid.NewGuid(), Name = "Иван Иванов" };
            var driver2 = new Driver { Id = Guid.NewGuid(), Name = "Петр Петров" };
            var client = new Client { Id = Guid.NewGuid(), PhoneNumber = "+79123456789" };
            var cargo = new Cargo { Id = Guid.NewGuid(), Name = "Груз" };
            var employee = new Employee { Id = Guid.NewGuid(), Name = "Сотрудник" };
            var route = new Route { Id = Guid.NewGuid(), Origin = "A", Destination = "B" };

            await _context.Drivers.AddRangeAsync(driver1, driver2);
            await _context.Clients.AddAsync(client);
            await _context.Cargos.AddAsync(cargo);
            await _context.Employees.AddAsync(employee);
            await _context.Routes.AddAsync(route);
            await _context.SaveChangesAsync();

            var orders = new List<Order>
            {
                new Order
                {
                    Id = Guid.NewGuid(),
                    DriverId = driver1.Id,
                    ClientId = client.Id,
                    CargoId = cargo.Id,
                    EmployeeId = employee.Id,
                    RouteId = route.Id,
                    Status = "В процессе"
                },
                new Order
                {
                    Id = Guid.NewGuid(),
                    DriverId = driver2.Id,
                    ClientId = client.Id,
                    CargoId = cargo.Id,
                    EmployeeId = employee.Id,
                    RouteId = route.Id,
                    Status = "Завершен"
                }
            };

            await _context.Orders.AddRangeAsync(orders);
            await _context.SaveChangesAsync();

            var mockViewManager = new Mock<IViewManager>();
            var viewModel = new OrderViewModel(_context, mockViewManager.Object)
            {
                DriverNameSearch = "Иван"
            };

            // Act
            await viewModel.SearchAsync();

            // Assert
            Assert.Single(viewModel.OrderList);
            Assert.Equal("Иван Иванов", viewModel.OrderList[0].Driver.Name);
        }

        [Fact]
        public async Task SearchAsync_NoResults_ReturnsEmptyList()
        {
            // Arrange
            _context = CreateInMemoryContext();

            var driver = new Driver { Id = Guid.NewGuid(), Name = "Иван Иванов" };
            var client = new Client { Id = Guid.NewGuid(), PhoneNumber = "+79123456789" };
            var cargo = new Cargo { Id = Guid.NewGuid(), Name = "Груз" };
            var employee = new Employee { Id = Guid.NewGuid(), Name = "Сотрудник" };
            var route = new Route { Id = Guid.NewGuid(), Origin = "A", Destination = "B" };

            await _context.Drivers.AddAsync(driver);
            await _context.Clients.AddAsync(client);
            await _context.Cargos.AddAsync(cargo);
            await _context.Employees.AddAsync(employee);
            await _context.Routes.AddAsync(route);
            await _context.SaveChangesAsync();

            var order = new Order
            {
                Id = Guid.NewGuid(),
                DriverId = driver.Id,
                ClientId = client.Id,
                CargoId = cargo.Id,
                EmployeeId = employee.Id,
                RouteId = route.Id,
                Status = "В процессе"
            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            var mockViewManager = new Mock<IViewManager>();
            var viewModel = new OrderViewModel(_context, mockViewManager.Object)
            {
                DriverNameSearch = "Несуществующий"
            };

            // Act
            await viewModel.SearchAsync();

            // Assert
            Assert.Empty(viewModel.OrderList);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
