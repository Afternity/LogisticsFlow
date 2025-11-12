using LogisticsFlow.ContractWPF.Common.ContractModels.MessageVMs;
using LogisticsFlow.ContractWPF.Common.Interfaces.InterfacesWindowManagers;
using LogisticsFlow.ContractWPF.ViewModels;
using LogisticsFlow.Domain.Models;
using LogisticsFlow.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsFlow.Tests.Tests.ViewModelTests.OrderViewModelTests
{
    public class OrderViewModelCreateTests 
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
        public async Task CreateAsync_ValidOrder_CreatesSuccessfully()
        {
            // Arrange
            _context = CreateInMemoryContext();

            // Создаем связанные сущности
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

            var mockViewManager = new Mock<IViewManager>();
            var viewModel = new OrderViewModel(_context, mockViewManager.Object)
            {
                OrderAdd = new Order
                {
                    DesiredDeliveryDate = DateTime.Now.AddDays(7),
                    Status = "Новый",
                    Price = 1500,
                    CargoId = cargo.Id,
                    EmployeeId = employee.Id,
                    ClientId = client.Id,
                    DriverId = driver.Id,
                    RouteId = route.Id
                }
            };

            var initialCount = await _context.Orders.CountAsync();

            // Act
            await viewModel.CreateAsync();

            // Assert
            var finalCount = await _context.Orders.CountAsync();
            Assert.Equal(initialCount + 1, finalCount);
            Assert.Equal(MessageState.Success, viewModel.Message.State);
            Assert.Contains("успешно создана", viewModel.Message.Message);
        }

        [Fact]
        public async Task CreateAsync_NullOrder_ShowsWarningMessage()
        {
            // Arrange
            _context = CreateInMemoryContext();
            var mockViewManager = new Mock<IViewManager>();
            var viewModel = new OrderViewModel(_context, mockViewManager.Object)
            {
                OrderAdd = null // Устанавливаем null
            };

            // Act
            await viewModel.CreateAsync();

            // Assert
            Assert.Equal(MessageState.Warning, viewModel.Message.State);
            Assert.Contains("не найден в интерфейсе", viewModel.Message.Message);
        }

        [Fact]
        public async Task CreateAsync_OrderWithEmptyId_CreatesWithNewGuid()
        {
            // Arrange
            _context = CreateInMemoryContext();

            var cargo = new Cargo { Id = Guid.NewGuid(), Name = "Груз" };
            var employee = new Employee { Id = Guid.NewGuid(), Name = "Сотрудник" };
            var client = new Client { Id = Guid.NewGuid(), PhoneNumber = "+79123456789" };
            var driver = new Driver { Id = Guid.NewGuid(), Name = "Водитель" };
            var route = new Route { Id = Guid.NewGuid(), Origin = "A", Destination = "B" };

            await _context.Cargos.AddAsync(cargo);
            await _context.Employees.AddAsync(employee);
            await _context.Clients.AddAsync(client);
            await _context.Drivers.AddAsync(driver);
            await _context.Routes.AddAsync(route);
            await _context.SaveChangesAsync();

            var mockViewManager = new Mock<IViewManager>();
            var viewModel = new OrderViewModel(_context, mockViewManager.Object)
            {
                OrderAdd = new Order
                {
                    Id = Guid.Empty, // Пустой Guid
                    DesiredDeliveryDate = DateTime.Now.AddDays(5),
                    Status = "В обработке",
                    Price = 2000,
                    CargoId = cargo.Id,
                    EmployeeId = employee.Id,
                    ClientId = client.Id,
                    DriverId = driver.Id,
                    RouteId = route.Id
                }
            };

            // Act
            await viewModel.CreateAsync();

            // Assert
            var createdOrder = await _context.Orders.FirstOrDefaultAsync();
            Assert.NotNull(createdOrder);
            Assert.NotEqual(Guid.Empty, createdOrder.Id); // Должен быть создан новый Guid
            Assert.Equal(MessageState.Success, viewModel.Message.State);
        }

        [Fact]
        public async Task CreateAsync_OrderWithExistingData_CopiesCorrectProperties()
        {
            // Arrange
            _context = CreateInMemoryContext();

            var cargo = new Cargo { Id = Guid.NewGuid(), Name = "Хрупкий груз" };
            var employee = new Employee { Id = Guid.NewGuid(), Name = "Менеджер" };
            var client = new Client { Id = Guid.NewGuid(), PhoneNumber = "+79998887766" };
            var driver = new Driver { Id = Guid.NewGuid(), Name = "Экспедитор" };
            var route = new Route { Id = Guid.NewGuid(), Origin = "Москва", Destination = "СПб" };

            await _context.Cargos.AddAsync(cargo);
            await _context.Employees.AddAsync(employee);
            await _context.Clients.AddAsync(client);
            await _context.Drivers.AddAsync(driver);
            await _context.Routes.AddAsync(route);
            await _context.SaveChangesAsync();

            var testDate = DateTime.Now.AddDays(10);
            var mockViewManager = new Mock<IViewManager>();
            var viewModel = new OrderViewModel(_context, mockViewManager.Object)
            {
                OrderAdd = new Order
                {
                    DesiredDeliveryDate = testDate,
                    Status = "Подготовка",
                    Price = 3000,
                    CargoId = cargo.Id,
                    EmployeeId = employee.Id,
                    ClientId = client.Id,
                    DriverId = driver.Id,
                    RouteId = route.Id
                }
            };

            // Act
            await viewModel.CreateAsync();

            // Assert
            var createdOrder = await _context.Orders
                .Include(o => o.Cargo)
                .Include(o => o.Driver)
                .FirstOrDefaultAsync();

            Assert.NotNull(createdOrder);
            Assert.Equal(testDate.Date, createdOrder.DesiredDeliveryDate.Date); // Проверяем дату
            Assert.Equal("Подготовка", createdOrder.Status);
            Assert.Equal(3000, createdOrder.Price);
            Assert.Equal(cargo.Id, createdOrder.CargoId);
            Assert.Equal(driver.Id, createdOrder.DriverId);
            Assert.Equal(MessageState.Success, viewModel.Message.State);
        }

        [Fact]
        public async Task CreateAsync_ResetsOrderAddAfterCreation()
        {
            // Arrange
            _context = CreateInMemoryContext();

            var cargo = new Cargo { Id = Guid.NewGuid(), Name = "Груз" };
            var employee = new Employee { Id = Guid.NewGuid(), Name = "Сотрудник" };
            var client = new Client { Id = Guid.NewGuid(), PhoneNumber = "+79123456789" };
            var driver = new Driver { Id = Guid.NewGuid(), Name = "Водитель" };
            var route = new Route { Id = Guid.NewGuid(), Origin = "A", Destination = "B" };

            await _context.Cargos.AddAsync(cargo);
            await _context.Employees.AddAsync(employee);
            await _context.Clients.AddAsync(client);
            await _context.Drivers.AddAsync(driver);
            await _context.Routes.AddAsync(route);
            await _context.SaveChangesAsync();

            var mockViewManager = new Mock<IViewManager>();
            var viewModel = new OrderViewModel(_context, mockViewManager.Object)
            {
                OrderAdd = new Order
                {
                    DesiredDeliveryDate = DateTime.Now.AddDays(3),
                    Status = "Новый",
                    Price = 1000,
                    CargoId = cargo.Id,
                    EmployeeId = employee.Id,
                    ClientId = client.Id,
                    DriverId = driver.Id,
                    RouteId = route.Id
                }
            };

            // Act
            await viewModel.CreateAsync();

            // Assert
            Assert.NotNull(viewModel.OrderAdd);
            Assert.Equal(Guid.Empty, viewModel.OrderAdd.Id); // Должен быть сброшен на новый объект
            Assert.Equal(0, viewModel.OrderAdd.Price); // Проверяем сброс цены
            Assert.Equal(MessageState.Success, viewModel.Message.State);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
