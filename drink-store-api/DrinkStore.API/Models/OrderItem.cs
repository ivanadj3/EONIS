namespace DrinkStore.API.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double TotalPrice { get; set; }
        public int Quantity { get; set; }
        public Order Order { get; set; }
    }
}
