using System.Collections;

namespace Funcky.Extensions;

public static partial class EnumerableExtensions
{
    /// <summary>
    /// Creates a buffer with a view over the source sequence, causing each enumerator to obtain access to all of the
    /// sequence's elements without causing multiple enumerations over the source.
    /// </summary>
    /// <typeparam name="TSource">Type of the elements in <paramref name="source"/> sequence.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <returns>A lazy buffer of the underlying sequence.</returns>
    [Pure]
    public static IBuffer<TSource> Memoize<TSource>(this IEnumerable<TSource> source)
        where TSource : notnull
        => source is IBuffer<TSource> buffer
            ? buffer
            : MemoizedBuffer.Create(source);

    private static class MemoizedBuffer
    {
        public static MemoizedBuffer<TSource> Create<TSource>(IEnumerable<TSource> source)
            => new(source);
    }

    private sealed class MemoizedBuffer<T> : IBuffer<T>
    {
        private readonly List<T> _buffer = new();
        private readonly IEnumerator<T> _source;

        private bool _disposed;

        public MemoizedBuffer(IEnumerable<T> source)
            => _source = source.GetEnumerator();

        public IEnumerator<T> GetEnumerator()
        {
            ThrowIfDisposed();

            return GetEnumeratorInternal();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            ThrowIfDisposed();

            return GetEnumeratorInternal();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _source.Dispose();
                _buffer.Clear();
                _disposed = true;
            }
        }

        private IEnumerator<T> GetEnumeratorInternal()
        {
            for (var index = 0; true; index++)
            {
                ThrowIfDisposed();

                if (index == _buffer.Count)
                {
                    if (_source.MoveNext())
                    {
                        _buffer.Add(_source.Current);
                    }
                    else
                    {
                        break;
                    }
                }

                yield return _buffer[index];
            }
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(MemoizedBuffer));
            }
        }
    }
}
