namespace MaxCo.Models
{
    public class OrderProductModel
    {
        public int OrderId { get; set; }
        public string? ProductName { get; set; }
        public decimal? ProductPrice { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
        public decimal? Total { get; set; }
    }
}
