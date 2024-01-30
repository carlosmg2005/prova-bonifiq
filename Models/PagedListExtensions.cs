namespace ProvaPub.Models
{
    public static class PagedListExtensions
    {
        public static async Task<CustomerList> ToCustomerList(this Task<PagedList<Customer>> pagedListTask)
        {
            var pagedList = await pagedListTask;
            return new CustomerList
            {
                HasNext = pagedList.HasNext,
                TotalCount = pagedList.TotalCount,
                Items = pagedList.Items.ToList()
            };
        }

        public static async Task<ProductList> ToProductList(this Task<PagedList<Product>> pagedListTask)
        {
            var pagedList = await pagedListTask;
            return new ProductList
            {
                HasNext = pagedList.HasNext,
                TotalCount = pagedList.TotalCount,
                Items = pagedList.Items.ToList()
            };
        }

    }
}
