using System.Diagnostics;

namespace Funcky.Monads
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    [DebuggerTypeProxy(typeof(EitherDebugView<,>))]
    public readonly partial struct Either<TLeft, TRight>
    {
        private string DebuggerDisplay => Match(
            left: _ => "Left",
            right: _ => "Right");
    }

    internal class EitherDebugView<TLeft, TRight>
    {
        private readonly Either<TLeft, TRight> _either;

        public EitherDebugView(Either<TLeft, TRight> either)
        {
            _either = either;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
        public object? Value => _either.Match(
            left: left => (object?)left,
            right: right => (object?)right);
    }
}
