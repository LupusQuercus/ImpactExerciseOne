namespace WebCart.Schemas
{
    public class OrderResponse
    {
        public string? OrderId { get; set; }
        public string? UserEmail { get; set; }
        public double TotalAmount { get; set; }
        public List<OrderLine>? OrderLines { get; set; }
    }
}
