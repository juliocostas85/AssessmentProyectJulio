namespace CarSales.Core.Domain.Entities
{
    public class Status
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public List<Order> Orders { get; set; } = new();
    }
}
