namespace SalesApplication.Requests.Sales;

public class CreateSaleRequest
{
    public long SaleNumber { get; set; }
    public DateTime Date { get; set; }
    public Guid CustomerId { get; set; }
    public Guid BranchId { get; set; }
    public List<Item> Items { get; set; } = [];

    public class Item
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}