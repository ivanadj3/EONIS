namespace DrinkStore.API.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public User User { get; set; }
        public string Address { get; set; }
        public IEnumerable<OrderItem> Items { get; set; }
        public bool Paid { get; set; }
    }
}
