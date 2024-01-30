using ProvaPub.Models;
using ProvaPub.Repository;

namespace ProvaPub.Services
{
	public class ProductService : ListService<Product>
	{
		public ProductService(TestDbContext ctx) : base(ctx) { }

		public async Task<ProductList>  ListProducts(int page, int pageSize = 10)
		{
			var query = _ctx.Products.AsQueryable();
			return await ListEntities(query, page, pageSize).ToProductList();
		}

	}
}
