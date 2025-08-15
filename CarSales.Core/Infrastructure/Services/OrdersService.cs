using CarSales.Core.Domain.DTOs;
using CarSales.Core.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarSales.Core.Infrastructure.Services
{
    public class OrdersService
    {
        private readonly AppDbContext dbContext;
        public OrdersService(AppDbContext dbContext) => this.dbContext = dbContext;

        public async Task<List<OrderDTO>> GetOrders(
            DateTime? dateFrom,
            DateTime? dateTo,
            List<int> customerIds,
            List<int> statusIds,
            bool? isActive)
        {
            var filterCustomers = customerIds != null && customerIds.Count > 0;
            var filterStatuses = statusIds != null && statusIds.Count > 0;

            var query = dbContext.Orders
                .AsNoTracking()
                .AsQueryable();

            if (dateFrom.HasValue)
                query = query.Where(o => o.OrderDate >= dateFrom.Value);

            if (dateTo.HasValue)
                query = query.Where(o => o.OrderDate <= dateTo.Value);

            if (filterCustomers)
                query = query.Where(o => customerIds!.Contains(o.CustomerId));

            if (filterStatuses)
                query = query.Where(o => statusIds!.Contains(o.StatusId));

            if (isActive.HasValue)
                query = query.Where(o => o.IsActive == isActive.Value);

            var results = await query
                .Select(o => new OrderDTO
                {
                    OrderId = o.Id,
                    OrderDate = o.OrderDate,
                    CustomerId = o.CustomerId,
                    CustomerName = o.Customer.Name,
                    StatusId = o.StatusId,
                    StatusName = o.Status.Name,
                    IsActive = o.IsActive,
                    Total = o.Total
                })
                .OrderBy(x => x.OrderId)
                .ToListAsync();

            return results;
        }
    }
}
