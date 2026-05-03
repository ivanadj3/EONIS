namespace DrinkStore.API.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
    }
}
