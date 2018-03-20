using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using Microsoft.Extensions.Logging;
using Moq;
using RedditSentimentAnalyzer.Analysis;
using Shouldly;
using Xunit;

namespace RedditSentimentAnalyzer.Tests.AzureSentimentAnalyzer
{
    public class GetSentimentAsyncTests
    {
        [Fact]
        public async Task GivenSomeContent_GetSentimentAsync_ReturnsExpectedResult()
        {
            // Arrange.
            var logger = new Mock<ILogger>();
            var textAnalyticsClient = new Mock<ITextAnalyticsApiWrapper>();
            var analyzer = new Analysis.AzureSentimentAnalyzer(logger.Object, textAnalyticsClient.Object);
            var content = new Dictionary<string, Tuple<string, double>>
            {
                {"this is great", new Tuple<string, double>("1", 0.9)},
                {"this is bad", new Tuple<string, double>("2", 0.1)}
            };

            var results = new SentimentBatchResult(content.Select(c => new SentimentBatchResultItem(c.Value.Item2, c.Value.Item1))
                                                          .ToList());
            textAnalyticsClient.Setup(x => x.SentimentAsync(It.IsAny<IList<MultiLanguageInput>>()))
                               .ReturnsAsync(results);

            // Act.
            var result = await analyzer.GetSentimentAsync(content.Keys);

            // Assert.
            textAnalyticsClient.VerifyAll();
            result.Keys.Count.ShouldBe(content.Count);
            foreach (var expected in content)
            {
                result.ShouldContainKeyAndValue(expected.Key, expected.Value.Item2);
            }
        }
    }
}