using Microsoft.EntityFrameworkCore;
using ProvaPub.Models;
using ProvaPub.Repository;

namespace ProvaPub.Services
{
    public class CustomerService : ListService<Customer>
    {
        private readonly TestDbContext _ctx;

        public CustomerService(TestDbContext ctx) : base(ctx)
        {
            _ctx = ctx;
        }

        public async Task<CustomerList> ListCustomers(int page, int pageSize = 10)
        {
            var query = _ctx.Customers.AsQueryable();
            return await ListEntities(query, page, pageSize).ToCustomerList();
        }

        public async Task<bool> CanPurchase(int customerId, decimal purchaseValue)
        {
            if (customerId <= 0) throw new ArgumentOutOfRangeException(nameof(customerId));

            if (purchaseValue <= 0) throw new ArgumentOutOfRangeException(nameof(purchaseValue));

            //Business Rule: Non registered Customers cannot purchase
            var customer = await _ctx.Customers.FindAsync(customerId);
            if (customer == null) throw new InvalidOperationException($"Customer Id {customerId} does not exists");

            //Business Rule: A customer can purchase only a single time per month
            var baseDate = DateTime.UtcNow.AddMonths(-1);
            var ordersInThisMonth = await _ctx.Orders.CountAsync(s => s.CustomerId == customerId && s.OrderDate >= baseDate);
            if (ordersInThisMonth > 0)
                return false;

            //Business Rule: A customer that never bought before can make a first purchase of maximum 100,00
            var haveBoughtBefore = await _ctx.Customers.CountAsync(s => s.Id == customerId && s.Orders.Any());
            if (haveBoughtBefore == 0 && purchaseValue > 100)
                return false;

            return true;
        }

        public async Task<Customer> GetCustomerById(int customerId)
        {
            return await _ctx.Customers.FindAsync(customerId);
        }

        /*public void UpdateCustomerOrders(int customerId, Order order)
        {
            var customer = _ctx.Customers.Include(c => c.Orders).FirstOrDefault(c => c.Id == customerId);

            if (customer != null)
            {
                //Adiciona o novo pedido à lista de pedidos do cliente
                customer.Orders.Add(order);

                //Atualiza o BD
                _ctx.SaveChanges();
            }
        }*/    
    }
}
