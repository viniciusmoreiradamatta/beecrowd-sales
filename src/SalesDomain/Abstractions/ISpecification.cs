namespace SalesDomain.Abstractions;

public interface ISpecification<T>
{
    bool IsSatisfiedBy(T item);
}
