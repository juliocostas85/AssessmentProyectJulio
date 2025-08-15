using CarSales.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarSales.Core.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Order> Orders => Set<Order>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Status> Statuses => Set<Status>();
    }
}
