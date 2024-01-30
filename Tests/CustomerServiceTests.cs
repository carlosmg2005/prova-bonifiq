using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using ProvaPub.Models;
using ProvaPub.Repository;
using ProvaPub.Services;
using Xunit;

namespace ProvaPub.Tests
{
    public class CustomerServiceTests
    {
        [Fact]
        public async Task CanPurchase_NonRegisteredCustomer_ReturnsFalse()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new TestDbContext(options))
            {
                var customerService = new CustomerService(context);

                // Act
                var result = await customerService.CanPurchase(0, 50);

                // Assert
                Assert.False(result);
            }
        }

        [Fact]
        public async Task CanPurchase_CustomerWithRecentPurchase_ReturnsFalse()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new TestDbContext(options))
            {
                var customerService = new CustomerService(context);

                // Simulate a customer with a recent purchase
                var customerId = 1;
                var order = new Order { CustomerId = customerId, OrderDate = DateTime.UtcNow };
                context.Orders.Add(order);
                await context.SaveChangesAsync();

                // Act
                var result = await customerService.CanPurchase(customerId, 100);

                // Assert
                Assert.False(result);
            }
        }

        [Fact]
        public async Task CanPurchase_FirstTimeCustomerExceedsMaxPurchaseValue_ReturnsFalse()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new TestDbContext(options))
            {
                var customerService = new CustomerService(context);

                // Simulate a first-time customer exceeding the max purchase value
                var customerId = 1;
                var purchaseValue = 150;
                var customer = new Customer { Id = customerId };
                context.Customers.Add(customer);
                await context.SaveChangesAsync();

                // Act
                var result = await customerService.CanPurchase(customerId, purchaseValue);

                // Assert
                Assert.False(result);
            }
        }

        // Adicionar mais testes para outras condições

    }
}
