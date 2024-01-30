public interface IPaymentMethod
{
    Task ProcessPayment(decimal paymentValue);
}

    public class PixPayment : IPaymentMethod
    {
        public async Task ProcessPayment(decimal paymentValue)
        {
            // Lógica específica para pagamento com Pix
            // Faz pagamento...
        }
    }

    public class CreditCardPayment : IPaymentMethod
    {
        public async Task ProcessPayment(decimal paymentValue)
        {
            // Lógica específica para pagamento com cartão de crédito
            // Faz pagamento...
        }
    }

    public class PayPalPayment : IPaymentMethod
    {
        public async Task ProcessPayment(decimal paymentValue)
        {
            // Lógica específica para pagamento com PayPal
            // Faz pagamento...
        }
    }