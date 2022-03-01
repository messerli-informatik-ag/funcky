using Funcky.Async.Extensions;
using Funcky.Async.Test.TestUtilities;
using Xunit;
using static Funcky.Functional;

namespace Funcky.Async.Test.Extensions.AsyncEnumerableExtensions
{
    public sealed class InspectTest
    {
        [Fact]
        public void InspectIsEnumeratedLazily()
        {
            var doNotEnumerate = new FailOnEnumerateAsyncSequence<object>();

            _ = doNotEnumerate.Inspect(NoOperation);
        }

        [Fact]
        public void InspectAwaitIsEnumeratedLazily()
        {
            var doNotEnumerate = new FailOnEnumerateAsyncSequence<object>();

            _ = doNotEnumerate.InspectAwait(static _ => ValueTask.CompletedTask);
        }

        [Fact]
        public async Task GivenAnAsyncEnumerableAndInjectWeCanApplySideEffectsToAsyncEnumerables()
        {
            var sideEffect = 0;
            var numbers = new List<int> { 1, 2, 3, 42 }.ToAsyncEnumerable();

            var numbersWithSideEffect = numbers
                .Inspect(n => { ++sideEffect; });

            Assert.Equal(0, sideEffect);

            await numbersWithSideEffect.ForEachAsync(NoOperation<int>);

            Assert.Equal(await numbers.CountAsync(), sideEffect);
        }

        [Fact]
        public async Task GivenAnAsyncEnumerableAndInjectAnAsynchronouseActionWeCanApplySideEffectsToAsyncEnumerables()
        {
            var sideEffect = 0;
            var numbers = new List<int> { 1, 2, 3, 42 }.ToAsyncEnumerable();

            var numbersWithSideEffect = numbers
                .InspectAwait(n =>
                {
                    ++sideEffect;
                    return default;
                });

            Assert.Equal(0, sideEffect);

            await numbersWithSideEffect.ForEachAsync(NoOperation<int>);

            Assert.Equal(await numbers.CountAsync(), sideEffect);
        }

        [Fact]
        public async Task CancellationIsPropagated()
        {
            var canceledToken = new CancellationToken(canceled: true);
            _ = await new AssertIsCancellationRequestedAsyncSequence<Unit>().Inspect(NoOperation).ToListAsync(canceledToken);
        }

        [Fact]
        public async Task CancellationIsPropagatedInAwaitOverload()
        {
            var canceledToken = new CancellationToken(canceled: true);
            _ = await new AssertIsCancellationRequestedAsyncSequence<Unit>().InspectAwait(_ => ValueTask.CompletedTask).ToListAsync(canceledToken);
        }
    }
}
