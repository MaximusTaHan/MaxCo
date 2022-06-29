namespace MaxCo.Models.ViewModels
{
    public class MaxCoViewModels
    {
        public List<ProductModel>? Products { get; set; }
        public List<OrdersModel>? Orders { get; set; }
        public List<OrderProductModel>? OrderProducts { get; set; }
        public List<FeatureProductModel>? FeaturedProducts { get; set; }
        public ProductModel? Product { get; set; }
        public OrderProductModel? OrderProduct { get; set; }
        public FinalizedOrder? FinalOrder { get; set; }
    }

    public class FinalizedOrder
    {
        public List<OrderProductModel>? OrderProducts { get; set; }
        public string? CustomerEmail { get; set; }
        public decimal? TotalPrice { get; set; }
    }
}
