using Funcky.Test.TestUtils;

namespace Funcky.Test;

public sealed class UnitTest
{
    [Fact]
    public void TheUnitTypeIsAlwaysTheSame()
    {
        Unit void1;
        Unit void2;
        var o = new object();

        Assert.Equal(void1, void2);
        Assert.NotEqual(void1, o);
    }

    [Fact]
    public void TransformAnActionToFunctionWithUnitReturn()
    {
        var unitAction = ActionToUnit<SideEffect>(AnAction);
        var sideEffect = new SideEffect();
        var unit = unitAction(sideEffect);

        Assert.Equal(default, unit);
        Assert.True(sideEffect.IsDone);
    }

    [Fact]
    public void TransformALambdaActionWith2ParametersToFunctionWithUnitReturn()
    {
        var unitAction = ActionToUnit((SideEffect effect, int p2) => effect.Do());
        var sideEffect = new SideEffect();
        var unit = unitAction(sideEffect, 42);

        Assert.Equal(default, unit);
        Assert.True(sideEffect.IsDone);
    }

    [Fact]
    public void TransformALambdaActionWith3ParametersToFunctionWithUnitReturn()
    {
        var unitAction = ActionToUnit((SideEffect effect, int p2, int p3) => effect.Do());
        var sideEffect = new SideEffect();
        var unit = unitAction(sideEffect, 0, 0);

        Assert.Equal(default, unit);
        Assert.True(sideEffect.IsDone);
    }

    [Fact]
    public void TransformALambdaActionWith4ParametersToFunctionWithUnitReturn()
    {
        var unitAction = ActionToUnit((SideEffect effect, int p2, int p3, int p4) => effect.Do());
        var sideEffect = new SideEffect();
        var unit = unitAction(sideEffect, 0, 0, 0);

        Assert.Equal(default, unit);
        Assert.True(sideEffect.IsDone);
    }

    [Fact]
    public void TransformALambdaActionWith5ParametersToFunctionWithUnitReturn()
    {
        var unitAction = ActionToUnit((SideEffect effect, int p2, int p3, int p4, int p5) => effect.Do());
        var sideEffect = new SideEffect();
        var unit = unitAction(sideEffect, 0, 0, 0, 0);

        Assert.Equal(default, unit);
        Assert.True(sideEffect.IsDone);
    }

    [Fact]
    public void TransformALambdaActionWith6ParametersToFunctionWithUnitReturn()
    {
        var unitAction = ActionToUnit((SideEffect effect, int p2, int p3, int p4, int p5, int p6) => effect.Do());
        var sideEffect = new SideEffect();
        var unit = unitAction(sideEffect, 0, 0, 0, 0, 0);

        Assert.Equal(default, unit);
        Assert.True(sideEffect.IsDone);
    }

    [Fact]
    public void TransformALambdaActionWith7ParametersToFunctionWithUnitReturn()
    {
        var unitAction = ActionToUnit((SideEffect effect, int p2, int p3, int p4, int p5, int p6, int p7) => effect.Do());
        var sideEffect = new SideEffect();
        var unit = unitAction(sideEffect, 0, 0, 0, 0, 0, 0);

        Assert.Equal(default, unit);
        Assert.True(sideEffect.IsDone);
    }

    [Fact]
    public void TransformALambdaActionWith8ParametersToFunctionWithUnitReturn()
    {
        var unitAction = ActionToUnit((SideEffect effect, int p2, int p3, int p4, int p5, int p6, int p7, int p8) => effect.Do());
        var sideEffect = new SideEffect();
        var unit = unitAction(sideEffect, 0, 0, 0, 0, 0, 0, 0);

        Assert.Equal(default, unit);
        Assert.True(sideEffect.IsDone);
    }

    [Fact]
    public void UnitValueIsTheSameAsDefaultUnit()
    {
        Assert.Equal(default, Unit.Value);
    }

    private static void AnAction(SideEffect sideEffect)
    {
        sideEffect.Do();
    }
}
