using System.Collections.Generic;
using Funcky.Extensions;
using Xunit;

namespace Funcky.Test
{
    public class Maybe
    {


        [Theory]
        [InlineData(-12, "-12")]
        [InlineData(0, "0")]
        [InlineData(-953542, "-953542")]
        [InlineData(1337, "1337")]
        public void ParseIntViaMaybeMonadWhereStringIsNumber(int parsed, string stringToParse)
        {
            var maybe = stringToParse.TryParseInt();

            Assert.True(maybe.Match(false, m => true));
            Assert.Equal(parsed, maybe.Match(0, m => m));
        }

        [Fact]
        public void ParseIntViaMaybeMonadWhereStringIsNoNumber()
        {
            var maybe = "no number".TryParseInt();

            Maybe<bool> isLeet = maybe.Select(m => m == 1337);

            Assert.False(isLeet.Match(false, b => true));
        }

        [Fact]
        public void GetValueFromDictionaryViaMaybeMonad()
        {
            var dictionary = new Dictionary<string, string> { { "some", "value" } };

            var maybe = dictionary.TryGetValue("some");

            Assert.True(maybe.Match(false, m => true));
            Assert.Equal("value", maybe.Match("", m => m));
        }

        [Fact]
        public void GetValueFromDictionaryViaMaybeMonadWhereKeyDoesNotExist()
        {
            var dictionary = new Dictionary<string, string> { { "some", "value" } };

            var maybe = dictionary.TryGetValue("none");

            Assert.False(maybe.Match(false, m => true));
        }
    }
}
