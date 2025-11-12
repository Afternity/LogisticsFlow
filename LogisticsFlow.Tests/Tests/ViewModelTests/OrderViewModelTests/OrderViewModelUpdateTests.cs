using LogisticsFlow.ContractWPF.Common.ContractModels.MessageVMs;
using LogisticsFlow.ContractWPF.Common.Interfaces.InterfacesWindowManagers;
using LogisticsFlow.ContractWPF.ViewModels;
using LogisticsFlow.Domain.Models;
using LogisticsFlow.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace LogisticsFlow.Tests.Tests.ViewModelTests.OrderViewModelTests
{
    public class OrderViewModelUpdateTests
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
        public async Task UpdateAsync_ValidOrder_UpdatesSuccessfully()
        {
            // Arrange
            _context = CreateInMemoryContext();

            // Создаем исходный заказ
            var originalOrder = new Order
            {
                Id = Guid.NewGuid(),
                OrderDate = DateTime.Now.AddDays(-5),
                DesiredDeliveryDate = DateTime.Now.AddDays(5),
                Status = "В процессе",
                Price = 1000
            };

            await _context.Orders.AddAsync(originalOrder);
            await _context.SaveChangesAsync();

            var mockViewManager = new Mock<IViewManager>();
            var viewModel = new OrderViewModel(_context, mockViewManager.Object)
            {
                OrderWorker = new Order
                {
                    Id = originalOrder.Id,
                    OrderDate = DateTime.Now.AddDays(-4),
                    DesiredDeliveryDate = DateTime.Now.AddDays(3),
                    Status = "Завершен",
                    Price = 1200
                }
            };

            // Act
            await viewModel.UpdateAsync();

            // Assert
            var updatedOrder = await _context.Orders.FindAsync(originalOrder.Id);
            Assert.NotNull(updatedOrder);
            Assert.Equal("Завершен", updatedOrder.Status);
            Assert.Equal(1200, updatedOrder.Price);
            Assert.Equal(MessageState.Success, viewModel.Message.State);
        }

        [Fact]
        public async Task UpdateAsync_NullOrder_ShowsWarningMessage()
        {
            // Arrange
            _context = CreateInMemoryContext();
            var mockViewManager = new Mock<IViewManager>();
            var viewModel = new OrderViewModel(_context, mockViewManager.Object)
            {
                OrderWorker = null
            };

            // Act
            await viewModel.UpdateAsync();

            // Assert
            Assert.Equal(MessageState.Warning, viewModel.Message.State);
            Assert.Contains("не найден в интерфейсе", viewModel.Message.Message);
        }

        [Fact]
        public async Task UpdateAsync_OrderWithEmptyId_ShowsWarningMessage()
        {
            // Arrange
            _context = CreateInMemoryContext();
            var mockViewManager = new Mock<IViewManager>();
            var viewModel = new OrderViewModel(_context, mockViewManager.Object)
            {
                OrderWorker = new Order { Id = Guid.Empty } // Пустой ID
            };

            // Act
            await viewModel.UpdateAsync();

            // Assert
            Assert.Equal(MessageState.Warning, viewModel.Message.State);
            Assert.Contains("не найден в интерфейсе", viewModel.Message.Message);
        }

        [Fact]
        public async Task UpdateAsync_NonExistentOrder_ShowsNotFoundMessage()
        {
            // Arrange
            _context = CreateInMemoryContext();
            var mockViewManager = new Mock<IViewManager>();
            var viewModel = new OrderViewModel(_context, mockViewManager.Object)
            {
                OrderWorker = new Order { Id = Guid.NewGuid() } // Несуществующий ID
            };

            // Act
            await viewModel.UpdateAsync();

            // Assert
            Assert.Equal(MessageState.Warning, viewModel.Message.State);
            Assert.Contains("Запись не найдена", viewModel.Message.Message);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}

