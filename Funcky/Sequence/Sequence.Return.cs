namespace Funcky;

public static partial class Sequence
{
    [Pure]
    public static IEnumerable<TItem> Return<TItem>(TItem item)
        => Enumerable.Repeat(item, 1);

    [Pure]
    public static IEnumerable<TItem> Return<TItem>(params TItem[] items)
        => items;
}
