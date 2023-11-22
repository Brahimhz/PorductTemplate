namespace Product.API.AppService.Dtos.Product
{
    public class ProductInPut
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public bool isActive { get; set; }
    }
}
