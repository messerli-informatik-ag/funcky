using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Funcky.Monads
{
    public readonly partial struct Option<TItem>
        where TItem : notnull
    {
        private readonly bool _hasItem;
        private readonly TItem _item;

        internal Option(TItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _item = item;
            _hasItem = true;
        }

        [Pure]
        public static bool operator ==(Option<TItem> lhs, Option<TItem> rhs) => lhs.Equals(rhs);

        [Pure]
        public static bool operator !=(Option<TItem> lhs, Option<TItem> rhs) => !lhs.Equals(rhs);

        [Pure]
        public static Option<TItem> None() => default;

        [Pure]
        public Option<TResult> Select<TResult>(Func<TItem, TResult> selector)
            where TResult : notnull
            => Match(
                 none: Option<TResult>.None,
                 item => Option.Some(selector(item)));

        [Pure]
        public Option<TResult> SelectMany<TResult>(Func<TItem, Option<TResult>> selector)
            where TResult : notnull
            => SelectMany(selector, (_, result) => result);

        [Pure]
        public Option<TResult> SelectMany<TMaybe, TResult>(Func<TItem, Option<TMaybe>> maybeSelector, Func<TItem, TMaybe, TResult> resultSelector)
            where TResult : notnull
            where TMaybe : notnull
            => Match(
                none: Option<TResult>.None,
                some: item => maybeSelector(item).Select(
                    maybe => resultSelector(item, maybe)));

        [Pure]
        public TResult Match<TResult>(TResult none, Func<TItem, TResult> some)
            => Match(() => none, some);

        [Pure]
        public TResult Match<TResult>(Func<TResult> none, Func<TItem, TResult> some)
            => _hasItem
                  ? some(_item)
                  : none();

        public void Match(Action none, Action<TItem> some)
        {
            if (_hasItem)
            {
                some(_item);
            }
            else
            {
                none();
            }
        }

        [Pure]
        public override bool Equals(object obj)
            => obj is Option<TItem> other && Equals(_item, other._item);

        [Pure]
        public override int GetHashCode()
            => Match(
                 none: 0,
                 some: item => item.GetHashCode());

        [Pure]
        public override string ToString()
            => Match(
                 none: "None",
                 some: value => $"Some({value})");
    }

    public static partial class Option
    {
        [Pure]
        public static Option<TItem> Some<TItem>(TItem item)
            where TItem : notnull
            => new Option<TItem>(item);

        [Pure]
        public static Option<TItem> Some<TItem>(Option<TItem> item)
            where TItem : notnull
            => item;
    }
}
