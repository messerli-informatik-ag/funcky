using System.Runtime.CompilerServices;
using Xunit.Sdk;

namespace Funcky.Test.Monads;

public sealed partial class ResultTest
{
    [Fact]
    public void CreateResultOkAndMatchCorrectly()
    {
        var value = Result.Ok(1000);

        var hasResult = value.Match(
            ok: True,
            error: False);

        Assert.True(hasResult);
    }

    [Fact]
    public void CreateResultErrorAndMatchCorrectly()
    {
        var value = Result<int>.Error(new ArgumentException());

        var hasResult = value.Match(
            ok: True,
            error: False);

        Assert.False(hasResult);
    }

    [Theory]
    [MemberData(nameof(GetIntegerResults))]
    public void CreateResultOkAndMatchASelectedResult(Result<int> value, double reference)
    {
        var doubleResult = value.Select(i => i * 0.25);

        var result = doubleResult.Match(
            ok: Identity,
            error: _ => -1.0);

        Assert.Equal(reference, result);
    }

    public static TheoryData<Result<int>, double> GetIntegerResults()
        => new()
        {
            { Result.Ok(1000), 250.0 },
            { Result.Ok(44), 11.0 },
            { Result.Ok(1), 0.25 },
            { Result.Ok(1000), 250.0 },
            { Result<int>.Error(new Exception()), -1.0 },
        };

    [Theory]
    [MemberData(nameof(GetIntegerSums))]
    public void TheSumsOverResultTypesShouldBeValid(Result<int> firstValue, Result<int> secondValue, Result<int> thirdValue, Option<int> referenceSum)
    {
        var result =
            from first in firstValue
            from second in secondValue
            from third in thirdValue
            select first + second + third;

        var resultSum = result.Match(
            ok: Option.Some,
            error: _ => Option<int>.None());

        Assert.Equal(referenceSum, resultSum);
    }

    public static TheoryData<Result<int>, Result<int>, Result<int>, Option<int>> GetIntegerSums()
        => new()
        {
            { Result.Ok(5), Result.Ok(10), Result.Ok(15), 30 },
            { Result.Ok(42), Result.Ok(1337), Result<int>.Error(new InvalidCastException()), Option<int>.None() },
            { Result.Ok(1337), Result.Ok(42), Result.Ok(99), 1478 },
            { Result.Ok(45856), Result.Ok(58788), Result.Ok(699554), 804198 },
            { Result<int>.Error(new InvalidCastException()), Result<int>.Error(new IOException()), Result<int>.Error(new MemberAccessException()), Option<int>.None() },
        };

    [Theory]
    [MemberData(nameof(GetResults))]
    public void MatchAcceptsActionsAsFunctions(Result<int> result, bool expected)
    {
        result
            .Match(
                ok: _ => Assert.True(expected),
                error: _ => Assert.False(expected));
    }

    public static TheoryData<Result<int>, bool> GetResults()
        => new()
        {
            { Result.Ok(5), true },
            { Result.Ok(42), true },
            { Result<int>.Error(new InvalidCastException()), false },
        };

    [Fact]
    public void GivenAResultWithAnExceptionWeGetAStackTrace()
    {
        const int arbitraryNumberOfStackFrames = 3;
        var exception = FunctionalAssert.IsError(InterestingStackTrace(arbitraryNumberOfStackFrames));
        Assert.NotNull(exception.StackTrace);
    }

    [Fact]
    public void GivenAResultWithAnExceptionTheStackTraceStartsInCreationMethod()
    {
        const int arbitraryNumberOfStackFrames = 0;
        var exception = FunctionalAssert.IsError(InterestingStackTrace(arbitraryNumberOfStackFrames));
        IsInterestingStackTraceFirst(exception);
    }

    [Fact]
    public void StackTraceIsPreservedWhenProjectingAResultUsingSelect()
    {
        const int arbitraryNumberOfStackFrames = 2;
        var result = InterestingStackTrace(arbitraryNumberOfStackFrames);

        var exception = FunctionalAssert.IsError(result);
        var stackTrace = exception.StackTrace;

        var exceptionAfterProjection = FunctionalAssert.IsError(result.Select(Identity));
        var stackTraceAfterProjection = exceptionAfterProjection.StackTrace;

        Assert.Equal(stackTrace, stackTraceAfterProjection);
    }

    [Fact]
    public void StackTraceIsPreservedWhenProjectingAResultUsingSelectMany()
    {
        const int arbitraryNumberOfStackFrames = 2;
        var result = InterestingStackTrace(arbitraryNumberOfStackFrames);

        var exception = FunctionalAssert.IsError(result);
        var stackTrace = exception.StackTrace;

        var exceptionAfterProjection = FunctionalAssert.IsError(result.SelectMany(Result.Return));
        var stackTraceAfterProjection = exceptionAfterProjection.StackTrace;

        Assert.Equal(stackTrace, stackTraceAfterProjection);
    }

    [Fact]
    public void SelectManyOnlyCallsSelectorWhenOk()
    {
        static Result<int> GetLength(string input) => Result.Ok(input.Length);
        var error = Result<string>.Error(new InvalidOperationException());
        var length =
            from a in error
            from b in GetLength(a)
            select b;
        FunctionalAssert.IsError(length);
    }

    [Fact]
    public void ErrorConstructorLeavesExistingStackTraceUnchanged()
    {
        var result = InterestingStackTrace(1);
        var expectedStackTraceString = FunctionalAssert.IsError(result).StackTrace;
        _ = result.Match(ok: Result.Ok, error: Result<int>.Error);
        var stackTraceString = FunctionalAssert.IsError(result).StackTrace;
        Assert.Equal(expectedStackTraceString, stackTraceString);
    }

    [Fact]
    public void SelectManyWithOkResultMatchesTherightValue()
        => FunctionalAssert.IsOk(2, Result.Ok(1).SelectMany(i => Result.Ok(i + 1)));

    [Fact]
    public void SelectManyWithErrorResultMatchesTherightValue()
        => FunctionalAssert.IsError(Result<int>.Error(new Exception("Any")).SelectMany(i => Result.Ok(i + 1)));

    [Fact]
    public void SelectManyReturnErrorResultWithOkResultMatchesTherightValue()
        => FunctionalAssert.IsError(Result.Ok(1).SelectMany(i => Result<int>.Error(new Exception("Any"))));

    [Fact]
    public void SelectManyReturnErrorResultWithErrorResultMatchesTherightValue()
        => FunctionalAssert.IsError(Result<int>.Error(new Exception("Any")).SelectMany(i => Result<int>.Error(new Exception("Other"))));

    [Fact]
    public void InspectDoesNothingWhenResultIsError()
    {
        var result = Result<string>.Error(new Exception());
        result.Inspect(_ => throw new XunitException("Side effect was unexpectedly called"));
    }

    [Fact]
    public void InspectCallsSideEffectWhenResultIsOk()
    {
        const int value = 10;
        var either = Result.Return(value);

        var sideEffect = Option<int>.None();
        either.Inspect(v => sideEffect = v);
        FunctionalAssert.IsSome(value, sideEffect);
    }

    [Theory]
    [MemberData(nameof(OkAndError))]
    public void InspectReturnsOriginalValue(Result<int> result)
    {
        Assert.Equal(result, result.Inspect(NoOperation));
    }

    public static TheoryData<Result<int>> OkAndError()
        => new() { Result<int>.Error(new Exception()), Result.Ok(123) };

    [Fact]
    public void CanBeCreatedImplicitlyFromOkValue()
    {
        static Unit AcceptsResult(Result<int> result) => Unit.Value;
        _ = AcceptsResult(10);
    }

    private static void IsInterestingStackTraceFirst(Exception exception)
    {
        Assert.NotNull(exception.StackTrace);

        var lines = exception.StackTrace.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        Assert.Matches(@"^\s+(\w+) Funcky\.Test\.Monads\.ResultTest\.InterestingStackTrace\s*\((System\.)?Int32 n\)", lines.First());
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private Result<int> InterestingStackTrace(int n)
        => n == 0
            ? Result<int>.Error(new InvalidCastException())
            : Indirection(n - 1);

    [MethodImpl(MethodImplOptions.NoInlining)]
    private Result<int> Indirection(int n)
        => InterestingStackTrace(n);
}
