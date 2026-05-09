namespace DrinkStore.API.Dto
{
    public class ReqMakeOrder
    {
        public IEnumerable<ReqMakeOrderItem> Items { get; set; }
    }

    public class ReqMakeOrderItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
