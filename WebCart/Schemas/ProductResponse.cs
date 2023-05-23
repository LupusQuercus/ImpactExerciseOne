namespace WebCart.Schemas
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public double Price { get; set; }
        public int Size { get; set; }
        public int Stars { get; set; }
    }
}
