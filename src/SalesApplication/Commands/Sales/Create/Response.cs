using SalesDomain.Entities.Sale;

namespace SalesApplication.Commands.Sales.Create;

public class Response
{
    public Guid Id { get; set; }
    public long SaleNumber { get; set; }
    public DateTime Date { get; set; }
    public Guid CustomerId { get; set; }
    public Guid BranchId { get; set; }
    public bool Cancelled { get; set; }
    public decimal TotalAmount { get; set; }
    public List<Item> Items { get; set; } = [];

    public static Response MapToResponse(Sale sale)
    {
        return new Response
        {
            Id = sale.Id,
            BranchId = sale.Id,
            CustomerId = sale.CustomerId,
            Date = sale.SaleDate,
            SaleNumber = sale.SaleNumber,
            TotalAmount = sale.TotalAmount,
            Cancelled = sale.Cancelled,
            Items = [.. sale.Items.Select(i => new Item { ProductId = i.ProductId, Quantity = i.Quantity, UnitPrice = i.UnitPrice })]
        };
    }

    public class Item
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}