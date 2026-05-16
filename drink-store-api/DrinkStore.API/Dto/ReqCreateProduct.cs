namespace DrinkStore.API.Dto
{
    public class ReqCreateProduct
    {
        public string Name { get; set; }
        public int Stock { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
    }
}
