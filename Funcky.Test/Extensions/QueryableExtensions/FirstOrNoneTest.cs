using Funcky.Test.TestUtils;

namespace Funcky.Test.Extensions.QueryableExtensions;

public sealed class FirstOrNoneTest
{
    [Fact]
    public void FirstOrNoneIsEvaluatedUsingExpressions()
        => _ = Enumerable.Empty<int>()
            .AsQueryable()
            .PreventAccidentalUseAsEnumerable()
            .FirstOrNone();
}
