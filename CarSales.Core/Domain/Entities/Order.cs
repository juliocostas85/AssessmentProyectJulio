namespace CarSales.Core.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;
        public int StatusId { get; set; }
        public Status Status { get; set; } = null!;
        public bool IsActive { get; set; }
        public decimal Total { get; set; }
    }
}
