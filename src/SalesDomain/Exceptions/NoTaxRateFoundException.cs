using SalesDomain.Entities.Sale;

namespace SalesDomain.Exceptions;
public class NoTaxRateFoundException(SaleItem item) : Exception($"No tax rate found for the item: {item.ProductId} Quantity: {item.Quantity}");
