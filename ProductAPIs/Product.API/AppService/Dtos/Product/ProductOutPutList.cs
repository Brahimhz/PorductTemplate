namespace Product.API.AppService.Dtos.Product
{
    public class ProductOutPutList
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public bool isActive { get; set; }
    }
}
