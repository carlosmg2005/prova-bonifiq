using ProvaPub.Models;
using System.Collections.Generic;

namespace ProvaPub.Services
{
    public class OrderService
    {
        private readonly Dictionary<string, IPaymentMethod> paymentMethods;
        private readonly CustomerService customerService;
        private int orderIdCounter = 1; // Um contador para gerar IDs de pedido únicos

        public OrderService(CustomerService customerService)
        {
            this.customerService = customerService;

            // Inicializa os métodos de pagamento disponíveis
            paymentMethods = new Dictionary<string, IPaymentMethod>
            {
                { "pix", new PixPayment() },
                { "creditcard", new CreditCardPayment() },
                { "paypal", new PayPalPayment() },
                // Adicione mais métodos de pagamento conforme necessário
            };
        }

        public async Task<Order> PayOrder(string paymentMethod, decimal paymentValue, int customerId)
        {
            if (paymentMethods.TryGetValue(paymentMethod, out var paymentMethodInstance))
            {
                // Utiliza a estratégia de pagamento adequada
                await paymentMethodInstance.ProcessPayment(paymentValue);
            }
            else
            {
                // Trate o caso em que o método de pagamento não é reconhecido
                throw new NotSupportedException("Método de pagamento não suportado");
            }

            // Gera um ID único para o pedido
            int orderId = GenerateOrderId();

            // Verifica se o cliente pode fazer a compra
            bool canPurchase = await customerService.CanPurchase(customerId, paymentValue);
            if (!canPurchase)
            {
                // Trate o caso em que o cliente não pode fazer a compra
                throw new InvalidOperationException($"O cliente com o ID {customerId} não pode fazer esta compra.");
            }

            Customer customer = await customerService.GetCustomerById(customerId);

            // Cria o objeto Order com valores apropriados
            Order order = new Order
            {
                Id = orderId,
                Value = paymentValue,
                CustomerId = customerId,
                OrderDate = DateTime.UtcNow, // Use a data atual (UTC) para o OrderDate
                Customer = customer
            };

            // Associa o pedido ao cliente
            order.CustomerId = customerId;

            // Adiciona o pedido à lista de pedidos do cliente
            //customerService.UpdateCustomerOrders(customerId, order);

            return order;
        }

        private int GenerateOrderId()
        {
            // Retorna um ID único para o pedido
            return Interlocked.Increment(ref orderIdCounter);
        }

        //Método GetCustomerById ao OrderService
        private async Task<Customer> GetCustomerById(int customerId)
        {
            return await customerService.GetCustomerById(customerId);
        }
    }

}
