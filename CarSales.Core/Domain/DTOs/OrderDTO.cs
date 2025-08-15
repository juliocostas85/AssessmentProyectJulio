namespace CarSales.Core.Domain.DTOs
{
    public class OrderDTO
    {
        public int OrderId { get; init; }
        public DateTime OrderDate { get; init; }
        public int CustomerId { get; init; }
        public string CustomerName { get; init; } = "";
        public int StatusId { get; init; }
        public string StatusName { get; init; } = "";
        public bool IsActive { get; init; }
        public decimal Total { get; init; }
    }
}
