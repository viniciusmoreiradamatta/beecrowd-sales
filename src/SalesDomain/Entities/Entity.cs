namespace SalesDomain.Entities;

public abstract class Entity(Guid id)
{
    public Guid Id { get; protected set; } = id;

    public List<string> Notifications { get; protected set; } = [];

    public bool Valid => Notifications.Count == 0;

    protected abstract void Validate();

    protected void ClearNotifications() => Notifications.Clear();
}