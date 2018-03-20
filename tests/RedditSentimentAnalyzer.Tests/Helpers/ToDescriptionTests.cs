using Shouldly;
using Xunit;

namespace RedditSentimentAnalyzer.Tests.Helpers
{
    public class ToDescriptionTests
    {
        [Theory]
        [InlineData(0.5, "Neutral")]
        [InlineData(1, "Positive")]
        [InlineData(0.6, "Positive")]
        [InlineData(0.4, "Negative")]
        [InlineData(0, "Negative")]
        public void GivenASentimentValue_ToDescription_ReturnsExpectedValue(double sentimentValue,
                                                                            string expectedValue)
        {
            sentimentValue.ToDescription()
                          .ShouldBe(expectedValue);
        }
    }
}