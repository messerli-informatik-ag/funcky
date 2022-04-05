using Funcky.Test.TestUtils;

namespace Funcky.Test.Extensions.EnumerableExtensions
{
    public sealed class TransposeTest
    {
        [Fact]
        public void TransposeIsLazyElementsGetOnlyEnumeratedWhenRequested()
        {
            const int numberOfRows = 5;
            const int numberOfColumns = 3;
            var lazyMatrix = LazyMatrix(numberOfRows, numberOfColumns);

            var transposedMatrix = lazyMatrix.Transpose();

            Assert.Equal(0, CountCreation.Count);

            transposedMatrix.ForEach(row => _ = row.ToList());

            Assert.Equal(numberOfRows * numberOfColumns, CountCreation.Count);
        }

        [Fact]
        public void TransposingAnEmptyMatrixResultsInAnEmptyMatrix()
        {
            var emptyMatrix = Enumerable.Empty<IEnumerable<int>>();

            var transposedMatrix = emptyMatrix.Transpose();

            Assert.Empty(transposedMatrix);
        }

        [Fact]
        public void TransposingAMatrixResultsInATransposedMatrix()
        {
            var transposed = MatrixExample().Transpose();

            Assert.Collection(
                transposed,
                row => { Assert.Equal(new[] { 1, 5, 9 }, row); },
                row => { Assert.Equal(new[] { 2, 6, 10 }, row); },
                row => { Assert.Equal(new[] { 3, 7, 11 }, row); },
                row => { Assert.Equal(new[] { 4, 8, 12 }, row); });
        }

        [Fact]
        public void GivenAMagicSquareTransposeDoesNotChangeTheAverages()
        {
            MagicSquare()
                .Select(Enumerable.Average)
                .ForEach(average => Assert.Equal(5, average));

            MagicSquare()
                .Transpose()
                .Select(Enumerable.Average)
                .ForEach(average => Assert.Equal(5, average));
        }

        [Fact]
        public void GivenAJaggedArrayTheTransposeDoesNotWorkAsExpected()
        {
            // Jagged sequences do not work!
            // If you use jagged sequences, in Transpose you are using an implementation detail which could change.
            var transposed = JaggedMatrixExample().Transpose();

            Assert.Collection(
                transposed,
                row => { Assert.Equal(new[] { 1, 6, 5, 10 }, row); },
                row => { Assert.Equal(new[] { 2, 9, 3, 42 }, row); },
                row => { Assert.Equal(new[] { 4 }, row); });
        }

        private static IEnumerable<IEnumerable<int>> MagicSquare() =>
            Sequence.Return(
                Sequence.Return(4, 9, 2),
                Sequence.Return(3, 5, 7),
                Sequence.Return(8, 1, 6));

        private static IEnumerable<IEnumerable<int>> MatrixExample() =>
            Sequence.Return(
                Sequence.Return(1, 2, 3, 4),
                Sequence.Return(5, 6, 7, 8),
                Sequence.Return(9, 10, 11, 12));

        private static IEnumerable<IEnumerable<int>> JaggedMatrixExample() =>
            Sequence.Return(
                Sequence.Return(1, 2, 3, 4),
                Sequence.Return(6, 9, 42),
                Sequence.Return(5),
                Sequence.Return(10));

        private static IEnumerable<IEnumerable<CountCreation>> LazyMatrix(int rows, int columns) =>
            from row in Enumerable.Range(0, rows)
            select from column in Enumerable.Range(0, columns)
                   select new CountCreation();
    }
}
