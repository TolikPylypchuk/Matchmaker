using FluentAssertions;

using Xunit;

namespace PatternMatching
{
    public class MatchTests
    {
        [Fact]
        public void MatchCreateShouldNeverReturnNull()
        {
            Match.Create<int, string>()
                .Should()
                .NotBeNull();

            Match.Create<int>()
                .Should()
                .NotBeNull();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void MatchCreateWithFallthroughShouldNeverReturnNull(bool fallthroughByDefault)
        {
            Match.Create<int, string>(fallthroughByDefault)
                .Should()
                .NotBeNull();

            Match.Create<int>(fallthroughByDefault)
                .Should()
                .NotBeNull();
        }
    }
}
