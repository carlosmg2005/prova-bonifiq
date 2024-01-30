using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using ProvaPub.Repository;
using System.Security.Cryptography;
using ProvaPub.Models;

namespace ProvaPub.Services
{
    public abstract class ListService<T> where T : class
    {
        protected readonly TestDbContext _ctx;

        public ListService(TestDbContext ctx)
        {
            _ctx = ctx;
        }

        protected async Task<PagedList<T>> ListEntities(IQueryable<T> query, int page, int pageSize)
        {
            var entities = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedList<T>
            {
                HasNext = entities.Count > page * pageSize,
                TotalCount = await query.CountAsync(),
                Items = entities
            };
        }
    }
}
