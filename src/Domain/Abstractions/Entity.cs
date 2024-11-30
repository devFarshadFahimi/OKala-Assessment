namespace Domain.Abstractions;

public abstract class Entity<TKey>
    where TKey : struct, IEquatable<TKey>
{
    public TKey Id { get; protected set; } = default!;

    protected Entity()
    {
    }

    #region Equality Check

    private bool Equals(Entity<TKey>? other) => other is not null && this == other;

    public override bool Equals(object? obj) =>
        obj is Entity<TKey> otherObject && Id.Equals(otherObject.Id);

    public override int GetHashCode() => Id.GetHashCode();

    public static bool operator ==(Entity<TKey>? left, Entity<TKey>? right)
    {
        return left is null && right is null || left is not null && right is not null && left.Equals(right);
    }

    public static bool operator !=(Entity<TKey> left, Entity<TKey> right)
        => !(right == left);

    #endregion
}

public abstract class LongEntity : Entity<long>
{
}