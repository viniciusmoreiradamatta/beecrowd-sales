using MediatR;
using SalesDomain.Abstractions.Response;

namespace SalesApplication.Commands.Sales.Create;

public class Command : IRequest<Result<Response>>
{
    public long SaleNumber { get; set; }
    public DateTime SaleDate { get; set; }
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