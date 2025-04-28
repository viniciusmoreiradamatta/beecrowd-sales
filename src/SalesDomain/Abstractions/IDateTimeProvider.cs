namespace SalesDomain.Abstractions;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow { get; } = DateTime.UtcNow;
}

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}