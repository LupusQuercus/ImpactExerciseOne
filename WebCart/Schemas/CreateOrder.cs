namespace WebCart.Schemas
{
    public class CreateOrder
    {
        public string? Token { get; set; }
        public string? UserEmail { get; set; }
        public double TotalAmount { get; set; }
    }
}
