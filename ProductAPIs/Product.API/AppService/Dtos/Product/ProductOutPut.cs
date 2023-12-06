namespace Product.API.AppService.Dtos.Product
{
    public class ProductOutPut
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public bool IsActive { get; set; }
        public Guid OwnerId { get; set; }
    }
}
