using LogisticsFlow.ContractWPF.Common.ContractModels.MessageVMs;
using LogisticsFlow.ContractWPF.Common.Interfaces.InterfacesWindowManagers;
using LogisticsFlow.ContractWPF.ViewModels;
using LogisticsFlow.Domain.Models;
using LogisticsFlow.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace LogisticsFlow.Tests.Tests.ViewModelTests.OrderViewModelTests
{
    public class OrderViewModelDeleteTests
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
        public async Task DeleteAsync_ValidOrder_DeletesSuccessfully()
        {
            // Arrange
            _context = CreateInMemoryContext();

            var orderToDelete = new Order
            {
                Id = Guid.NewGuid(),
                Status = "В процессе",
                Price = 1000
            };

            await _context.Orders.AddAsync(orderToDelete);
            await _context.SaveChangesAsync();

            var initialCount = await _context.Orders.CountAsync();

            var mockViewManager = new Mock<IViewManager>();
            var viewModel = new OrderViewModel(_context, mockViewManager.Object)
            {
                OrderWorker = new Order { Id = orderToDelete.Id }
            };

            // Act
            await viewModel.DeleteAsync();

            // Assert
            var finalCount = await _context.Orders.CountAsync();
            Assert.Equal(initialCount - 1, finalCount);
            Assert.Equal(MessageState.Success, viewModel.Message.State);
            Assert.Contains("успешно удалена", viewModel.Message.Message);
        }

        [Fact]
        public async Task DeleteAsync_NullOrder_ShowsWarningMessage()
        {
            // Arrange
            _context = CreateInMemoryContext();
            var mockViewManager = new Mock<IViewManager>();
            var viewModel = new OrderViewModel(_context, mockViewManager.Object)
            {
                OrderWorker = null
            };

            // Act
            await viewModel.DeleteAsync();

            // Assert
            Assert.Equal(MessageState.Warning, viewModel.Message.State);
            Assert.Contains("не найден в интерфейсе", viewModel.Message.Message);
        }

        [Fact]
        public async Task DeleteAsync_OrderWithEmptyId_ShowsWarningMessage()
        {
            // Arrange
            _context = CreateInMemoryContext();
            var mockViewManager = new Mock<IViewManager>();
            var viewModel = new OrderViewModel(_context, mockViewManager.Object)
            {
                OrderWorker = new Order { Id = Guid.Empty }
            };

            // Act
            await viewModel.DeleteAsync();

            // Assert
            Assert.Equal(MessageState.Warning, viewModel.Message.State);
            Assert.Contains("не найден в интерфейсе", viewModel.Message.Message);
        }

        [Fact]
        public async Task DeleteAsync_NonExistentOrder_ShowsNotFoundMessage()
        {
            // Arrange
            _context = CreateInMemoryContext();
            var mockViewManager = new Mock<IViewManager>();
            var viewModel = new OrderViewModel(_context, mockViewManager.Object)
            {
                OrderWorker = new Order { Id = Guid.NewGuid() } // Несуществующий ID
            };

            // Act
            await viewModel.DeleteAsync();

            // Assert
            Assert.Equal(MessageState.Warning, viewModel.Message.State);
            Assert.Contains("Запись не найдена", viewModel.Message.Message);
        }

        [Fact]
        public async Task DeleteAsync_ResetsOrderWorkerAfterDeletion()
        {
            // Arrange
            _context = CreateInMemoryContext();

            var orderToDelete = new Order
            {
                Id = Guid.NewGuid(),
                Status = "В процессе",
                Price = 1000
            };

            await _context.Orders.AddAsync(orderToDelete);
            await _context.SaveChangesAsync();

            var mockViewManager = new Mock<IViewManager>();
            var viewModel = new OrderViewModel(_context, mockViewManager.Object)
            {
                OrderWorker = new Order { Id = orderToDelete.Id }
            };

            // Act
            await viewModel.DeleteAsync();

            // Assert
            Assert.NotNull(viewModel.OrderWorker);
            Assert.Equal(Guid.Empty, viewModel.OrderWorker.Id); // Должен быть сброшен
            Assert.Equal(MessageState.Success, viewModel.Message.State);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
