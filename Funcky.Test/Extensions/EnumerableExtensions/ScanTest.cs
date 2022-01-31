using FsCheck;
using FsCheck.Xunit;
using Funcky.Test.TestUtils;

namespace Funcky.Test.Extensions.AsyncEnumerableExtensions
{
    public class ScanTest
    {
        [Fact]
        public void InclusiveScanIsEnumeratedLazily()
        {
            var doNotEnumerate = new FailOnEnumerationSequence<int>();

            _ = doNotEnumerate.InclusiveScan(0, AddElement);
        }

        [Property]
        public Property InclusiveScanOnAnEmptyListReturnsAnEmptyList(int neutral, Func<int, int, int> func)
        {
            var empty = Enumerable.Empty<int>();

            return empty.InclusiveScan(neutral, func).None().ToProperty();
        }

        [Fact]
        public void InclusiveScanCalculatesInclusivePrefixSum()
        {
            var numbers = new List<int> { 1, 5, 7, 42, 1337 };
            var inclusivePrefixSum = new List<int> { 1, 6, 13, 55, 1392 };

            Assert.Equal(inclusivePrefixSum, numbers.InclusiveScan(0, AddElement));
        }

        [Fact]
        public void ExclusiveScanIsEnumeratedLazily()
        {
            var doNotEnumerate = new FailOnEnumerationSequence<int>();

            _ = doNotEnumerate.ExclusiveScan(0, AddElement);
        }

        [Property]
        public Property ExclusiveScanOnAnEmptyListReturnsAnEmptyList(int neutral, Func<int, int, int> func)
        {
            var empty = Enumerable.Empty<int>();

            return empty.ExclusiveScan(neutral, func).None().ToProperty();
        }

        [Fact]
        public void ExclusiveScanCalculatesExclusivePrefixSum()
        {
            var numbers = new List<int> { 1, 5, 7, 42, 1337 };
            var exclusivePrefixSum = new List<int> { 0, 1, 6, 13, 55 };

            Assert.Equal(exclusivePrefixSum, numbers.ExclusiveScan(0, AddElement));
        }

        private static int AddElement(int sum, int element)
            => sum + element;
    }
}
