using SalesDomain.Abstractions;
using SalesDomain.Validators.Sales;

namespace SalesDomain.Entities.Sale;

public class Sale : Entity
{
    protected Sale() : base(Guid.NewGuid())
    {
    }

    private Sale(DateTime saleDate, Guid customerId, Guid branchId, bool cancelled, List<SaleItem> items, Guid? id, long saleNumber) : base(id ?? Guid.NewGuid())
    {
        SaleDate = saleDate;
        CustomerId = customerId;
        BranchId = branchId;
        Cancelled = cancelled;
        Items = items;
        SaleNumber = saleNumber;
    }

    public long SaleNumber { get; private set; }
    public DateTime SaleDate { get; private set; }
    public Guid CustomerId { get; private set; }
    public Guid BranchId { get; private set; }
    public decimal TotalAmount { get; private set; }
    public bool Cancelled { get; private set; }
    public List<SaleItem> Items { get; private set; }

    protected override void Validate()
    {
        ClearNotifications();

        var validator = new SaleValidator();

        var result = validator.Validate(this);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
            {
                Notifications.Add(error.ErrorMessage);
            }
        }

        if (Items.Any(c => !c.Valid))
        {
            Notifications.AddRange(Items.SelectMany(c => c.Notifications));
        }
    }

    public void Cancel() => Cancelled = true;

    public static Sale CreateSale(IDateTimeProvider provider, Guid customerId, Guid branchId, List<SaleItem> items, Guid id, long saleNumber = 0)
    {
        var sale = new Sale(provider.UtcNow, customerId, branchId, false, items, id, saleNumber);

        sale.TotalAmount = sale.Items.Sum(i => i.Total);

        sale.Validate();

        return sale;
    }
}