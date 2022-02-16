using Funcky.Internal;

namespace Funcky.Extensions
{
    public static partial class EnumerableExtensions
    {
        /// <summary>
        /// Returns the given sequence eagerly in random Order in O(n).
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the enumerable.</typeparam>
        /// <remarks>This method is implemented by using deferred execution. The immediate return value is an object that stores all the information that is required to perform the action. The query represented by this method is not executed until the object is enumerated either by calling its GetEnumerator method directly or by using foreach.</remarks>
        [Pure]
        public static IReadOnlyList<TSource> Shuffle<TSource>(this IEnumerable<TSource> source)
            where TSource : notnull
            => source
                .ToList()
                .ToRandomList(new Random());

        /// <summary>
        /// Returns the given sequence eagerly in random Order in O(n).
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the enumerable.</typeparam>
        /// <remarks>This method is implemented by using deferred execution. The immediate return value is an object that stores all the information that is required to perform the action. The query represented by this method is not executed until the object is enumerated either by calling its GetEnumerator method directly or by using foreach.</remarks>
        [Pure]
        public static IReadOnlyList<TSource> Shuffle<TSource>(this IEnumerable<TSource> source, Random random)
            where TSource : notnull
            => source
                .ToList()
                .ToRandomList(random);

        /// <summary>
        /// Returns the given sequence eagerly in random Order in O(n).
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the enumerable.</typeparam>
        /// <remarks>This method is implemented by using deferred execution. The immediate return value is an object that stores all the information that is required to perform the action. The query represented by this method is not executed until the object is enumerated either by calling its GetEnumerator method directly or by using foreach.</remarks>
        [Pure]
        public static IReadOnlyList<TSource> Shuffle<TSource>(this IList<TSource> source)
            => source
                .ToRandomList(new Random());

        /// <summary>
        /// Returns the given sequence eagerly in random Order in O(n).
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the enumerable.</typeparam>
        /// <remarks>This method is implemented by using deferred execution. The immediate return value is an object that stores all the information that is required to perform the action. The query represented by this method is not executed until the object is enumerated either by calling its GetEnumerator method directly or by using foreach.</remarks>
        [Pure]
        public static IReadOnlyList<TSource> Shuffle<TSource>(this IList<TSource> source, Random random)
            => source
                .ToRandomList(random);
    }
}
