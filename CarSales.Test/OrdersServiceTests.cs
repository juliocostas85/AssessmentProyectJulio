
using CarSales.Core.Domain.Entities;
using CarSales.Core.Infrastructure.Context;
using CarSales.Core.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;


namespace CarSales.Test;
public class OrdersServiceTests
{
    private static AppDbContext BuildContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .EnableSensitiveDataLogging()
            .Options;

        var ctx = new AppDbContext(options);

        if (!ctx.Customers.Any())
        {
           
            var c1 = new Customer { Id = 1, Name = "Alicia" };
            var c2 = new Customer { Id = 2, Name = "Julio" };
            var s1 = new Status { Id = 1, Name = "Pending" };
            var s2 = new Status { Id = 2, Name = "Completed" };

            ctx.Customers.AddRange(c1, c2);
            ctx.Statuses.AddRange(s1, s2);

            ctx.Orders.AddRange(
                new Order { Id = 1, OrderDate = new DateTime(2025, 1, 5, 10, 0, 0), CustomerId = 1, StatusId = 1, IsActive = true, Total = 100, Customer = c1, Status = s1 },
                new Order { Id = 2, OrderDate = new DateTime(2025, 1, 10, 8, 0, 0), CustomerId = 2, StatusId = 1, IsActive = false, Total = 200, Customer = c2, Status = s1 },
                new Order { Id = 3, OrderDate = new DateTime(2025, 2, 1, 12, 0, 0), CustomerId = 1, StatusId = 2, IsActive = true, Total = 300, Customer = c1, Status = s2 }
            );

            ctx.SaveChanges();
        }

        return ctx;
    }

    [Fact]
    public async Task GetOrders_NoFilters_ReturnsAll()
    {
        using var ctx = BuildContext("GetOrders_NoFilters_ReturnsAll");
        var svc = new OrdersService(ctx);

        var result = await svc.GetOrders(null, null, null!, null!, null);

        Assert.Equal(3, result.Count);
        Assert.Equal(new[] { 1, 2, 3 }, result.Select(x => x.OrderId).ToArray());
    }

    [Fact]
    public async Task GetOrders_FilterByDateRange_Inclusive()
    {
        using var ctx = BuildContext("GetOrders_FilterByDateRange_Inclusive");
        var svc = new OrdersService(ctx);

       
        var result = await svc.GetOrders(
            new DateTime(2025, 1, 1),
            new DateTime(2025, 1, 10, 23, 59, 59), 
            null!, null!, null);

        Assert.Equal(new[] { 1, 2 }, result.Select(x => x.OrderId).ToArray());
    }

    [Fact]
    public async Task GetOrders_FilterByCustomers()
    {
        using var ctx = BuildContext("GetOrders_FilterByCustomers");
        var svc = new OrdersService(ctx);

        var result = await svc.GetOrders(null, null, new List<int> { 1 }, null!, null);

        Assert.Equal(new[] { 1, 3 }, result.Select(x => x.OrderId).ToArray());
        Assert.All(result, x => Assert.Equal(1, x.CustomerId));
    }

    [Fact]
    public async Task GetOrders_FilterByStatuses()
    {
        using var ctx = BuildContext("GetOrders_FilterByStatuses");
        var svc = new OrdersService(ctx);

        var result = await svc.GetOrders(null, null, null!, new List<int> { 1 }, null);

        Assert.Equal(new[] { 1, 2 }, result.Select(x => x.OrderId).ToArray());
        Assert.All(result, x => Assert.Equal(1, x.StatusId));
    }

    [Fact]
    public async Task GetOrders_FilterByIsActive()
    {
        using var ctx = BuildContext("GetOrders_FilterByIsActive");
        var svc = new OrdersService(ctx);

        var result = await svc.GetOrders(null, null, null!, null!, true);

        Assert.Equal(new[] { 1, 3 }, result.Select(x => x.OrderId).ToArray());
        Assert.All(result, x => Assert.True(x.IsActive));
    }

    [Fact]
    public async Task GetOrders_CombinedFilters()
    {
        using var ctx = BuildContext("GetOrders_CombinedFilters");
        var svc = new OrdersService(ctx);

        var result = await svc.GetOrders(
            new DateTime(2025, 1, 1),
            new DateTime(2025, 12, 31),
            new List<int> { 1 },        
            new List<int> { 2 },      
            true                        
        );

        
        var item = Assert.Single(result);
        Assert.Equal(3, item.OrderId);
        Assert.Equal("Alice", item.CustomerName);
        Assert.Equal("Completed", item.StatusName);
        Assert.True(item.IsActive);
    }
}
