using System.Reflection;

namespace Domain.Abstractions;

public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot
    where TId : struct,
          IEquatable<TId>
{
    private readonly List<IDomainEvent> _events = [];
    protected AggregateRoot() => _events = [];
    protected AggregateRoot(IEnumerable<IDomainEvent> events)
    {
        var domainEvents = events.ToList();
        if (domainEvents.Count == 0) return;
        foreach (var @event in domainEvents)
        {
            Mutate(@event);
        }
    }

    protected void Apply(IDomainEvent @event)
    {
        Mutate(@event);
        AddEvent(@event);
    }
    protected void AddEvent(IDomainEvent @event) => _events.Add(@event);

    protected void Mutate(IDomainEvent @event)
    {
        var onMethod = GetType().GetMethod("On", BindingFlags.Instance | BindingFlags.NonPublic, [@event.GetType()]);
        onMethod?.Invoke(this, [@event]);
    }

    public IEnumerable<IDomainEvent> GetEvents() => _events.AsEnumerable();

    public void ClearEvents() => _events.Clear();
}



public abstract class AggregateRoot : AggregateRoot<long>
{

}

