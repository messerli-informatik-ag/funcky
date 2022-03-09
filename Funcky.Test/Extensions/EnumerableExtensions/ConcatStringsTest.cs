namespace Funcky.Test.Extensions.EnumerableExtensions;

public class ConcatStringsTest
{
    [Fact]
    public void ConcatenatingAnEmptySetOfStringsReturnsAnEmptyString()
    {
        var empty = Enumerable.Empty<string>();

        Assert.Equal(string.Empty, empty.ConcatToString());
    }

    [Fact]
    public void ConcatenatingASetWithExactlyOneElementReturnsTheElement()
    {
        var singleElement = Sequence.Return("Alpha");

        Assert.Equal("Alpha", singleElement.ConcatToString());
    }

    [Fact]
    public void ConcatenatingAListOfStringsReturnsAllElementsWithoutASeparator()
    {
        var strings = new List<string> { "Alpha", "Beta", "Gamma" };

        Assert.Equal("AlphaBetaGamma", strings.ConcatToString());
    }

    [Fact]
    public void ConcatenatingNonStringsWorksToo()
    {
        var strings = new List<int> { 1, 2, 3 };

        Assert.Equal("123", strings.ConcatToString());
    }

    [Fact]
    public void NullsAreHandledAsEmptyStringsWhileConcatenating()
    {
        var strings = new List<string?> { "Alpha", null, "Gamma" };

        Assert.Equal("AlphaGamma", strings.ConcatToString());
    }
}
