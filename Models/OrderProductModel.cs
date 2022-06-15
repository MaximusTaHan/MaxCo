namespace MaxCo.Models
{
    public class OrderProductModel
    {
        public int OrderId { get; set; }
        public string? ProductName { get; set; }
        public decimal? ProductPrice { get; set; }
        public int? Quantity { get; set; }
    }
}
