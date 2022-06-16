namespace MaxCo.Models.ViewModels
{
    public class MaxCoViewModels
    {
        public List<ProductModel>? Products { get; set; }
        public List<OrdersModel>? Orders { get; set; }
        public List<OrderProductModel>? OrderProducts { get; set; }
        public ProductModel? Product { get; set; }
        public OrderProductModel? OrderProduct { get; set; }
    }
}
