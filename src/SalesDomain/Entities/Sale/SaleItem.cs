using SalesDomain.Entities.Sale.Specifications.TaxSpecifications;
using SalesDomain.Validators.Sales;

namespace SalesDomain.Entities.Sale;

public class SaleItem : Entity
{
    public readonly decimal SpecialIva = 0.20m;
    public readonly decimal StandartIva = 0.10m;

    private SaleItem(Guid id, Guid productId, Guid saleId, int quantity, decimal unitPrice, bool isCancelled) : base(id)
    {
        Id = id;
        ProductId = productId;
        SaleId = saleId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        IsCancelled = isCancelled;
    }

    public Guid ProductId { get; private set; }
    public Guid SaleId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal ValueMonetaryTaxApplied { get; private set; }
    public decimal Total { get; private set; }
    public bool IsCancelled { get; private set; }

    private void ApplyTaxes()
    {
        var taxRate = new TaxRateSpecification().GetTaxRate(this);

        ValueMonetaryTaxApplied = UnitPrice * Quantity * taxRate;
        Total = UnitPrice * Quantity + ValueMonetaryTaxApplied;
    }

    public static SaleItem CreateSaleItem(Guid productId, Guid saleId, int quantity, decimal unitPrice, bool isCancelled, Guid? id = null)
    {
        var item = new SaleItem(id ?? Guid.NewGuid(), productId, saleId, quantity, unitPrice, isCancelled);

        item.ApplyTaxes();

        item.Validate();

        return item;
    }

    protected override void Validate()
    {
        ClearNotifications();

        var validator = new SaleItemValidator();

        var result = validator.Validate(this);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
            {
                Notifications.Add(error.ErrorMessage);
            }
        }
    }
}