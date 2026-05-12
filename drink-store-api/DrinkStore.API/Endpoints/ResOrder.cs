namespace DrinkStore.API.Endpoints
{
    public class ResOrder
    {
        public int Id { get; set; }
        public double TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public IEnumerable<ResOrderItem> Items { get; set; }
        public bool Paid { get; set; }
    }

    public class ResOrderItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double TotalPrice { get; set; }
        public int Quantity { get; set; }
    }
}
