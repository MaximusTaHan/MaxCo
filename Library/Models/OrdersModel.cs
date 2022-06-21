namespace MaxCo.Models
{
    public class OrdersModel
    {
        public int OrderId { get; set; }
        public int OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string? UserId { get; set; }
        public int OrderStatus { get; set; }
    }
}
