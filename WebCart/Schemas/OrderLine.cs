namespace WebCart.Schemas
{
    public class OrderLine
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public double ProductUnitPrice { get; set; }
        public string? ProductSize { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
    }
}
