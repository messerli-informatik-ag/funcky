namespace Funcky.Monads;

public static partial class OptionExtensions
{
    public static Either<TLeft, TRight> ToEither<TLeft, TRight>(this Option<TRight> option, TLeft left)
        where TLeft : notnull
        where TRight : notnull
        => option.Match(
            none: Either<TLeft, TRight>.Left(left),
            some: Either<TLeft, TRight>.Right);

    public static Either<TLeft, TRight> ToEither<TLeft, TRight>(this Option<TRight> option, Func<TLeft> left)
        where TLeft : notnull
        where TRight : notnull
    {
        var toLeft = Either<TLeft, TRight>.Left;

        return option.Match(
            none: toLeft.Compose(left),
            some: Either<TLeft, TRight>.Right);
    }
}
