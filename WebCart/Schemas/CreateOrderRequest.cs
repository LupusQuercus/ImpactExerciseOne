namespace WebCart.Schemas
{
    public class CreateOrderRequest
    {
        public string? UserEmail { get; set; }
        public double TotalAmount { get; set; }
        public List<OrderLine>? OrderLines { get; set; }
    }
}
