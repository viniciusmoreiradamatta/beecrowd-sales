namespace SalesApplication.Commands.Sales.GetAll;

public class Response
{
    public Guid Id { get; set; }
    public long SaleNumber { get; set; }
    public DateTime Date { get; set; }
    public Guid CustomerId { get; set; }
    public Guid BranchId { get; set; }
    public decimal TotalAmount { get; set; }
    public bool Cancelled { get; set; }
    public List<Item> Items { get; set; }

    public class Item
    {
        public Guid Id { get; set; }
        public Guid SaleId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal ValueMonetaryTaxApplied { get; set; }
        public decimal Total { get; set; }
        public bool IsCancelled { get; set; }
    }
}