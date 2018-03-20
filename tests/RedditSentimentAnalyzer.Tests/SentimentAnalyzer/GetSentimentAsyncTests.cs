using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using RedditSentimentAnalyzer.Analysis;
using RedditSentimentAnalyzer.RedditSearch;
using Shouldly;
using Xunit;

namespace RedditSentimentAnalyzer.Tests.SentimentAnalyzer
{
    public class GetSentimentAsyncTests
    {
        [Fact]
        public async Task GivenASearchTermWithNothingConfigured_GetSentimentAsync_ReturnsEmtpyResult()
        {
            // Arrange.
            var logger = new Mock<ILogger<RedditSentimentAnalyzer.SentimentAnalyzer>>();
            var sentimentAnalyzer = new Mock<IAzureSentimentAnalyzer>();
            var redditSearch = new Mock<IRedditSearch>();
            var analyzer = new RedditSentimentAnalyzer.SentimentAnalyzer(logger.Object, sentimentAnalyzer.Object, redditSearch.Object);
            const string subreddit = "cryptocurrency";
            const string searchTerm = "bitcoin";

            // Act.
            var results = await analyzer.GetSentimentAsync(subreddit, searchTerm);

            // Assert.
            sentimentAnalyzer.Verify(x => x.GetSentimentAsync(It.IsAny<IEnumerable<string>>()), Times.Never);

            results.ShouldBeNull();
        }

        [Fact]
        public async Task GivenASearchTermAndRedditConfigured_GetSentimentAsync_ReturnsResults()
        {
            // Arrange.
            var logger = new Mock<ILogger<RedditSentimentAnalyzer.SentimentAnalyzer>>();
            var sentimentAnalyzer = new Mock<IAzureSentimentAnalyzer>();
            var redditSearch = new Mock<IRedditSearch>();
            var analyzer = new RedditSentimentAnalyzer.SentimentAnalyzer(logger.Object, sentimentAnalyzer.Object, redditSearch.Object);
            const string subreddit = "cryptocurrency";
            const string searchTerm = "bitcoin";

            var redditResults = new[]
            {
                new SearchResult
                {
                    Title = "title",
                    Content = "content",
                    Permalink = "xyz"
                },
                new SearchResult
                {
                    Title = "title2",
                    Content = "content2",
                    Permalink = "xyz2"
                },
                new SearchResult
                {
                    Title = "title3",
                    Content = "content3",
                    Permalink = "xyz3"
                }
            };
            redditSearch.Setup(x => x.SearchAsync(subreddit, searchTerm, 100))
                        .ReturnsAsync(redditResults);

            var random = new Random();
            var sentiments = redditResults.ToDictionary(key => $"{key.Title}. {key.Content}", value => random.NextDouble());
            sentimentAnalyzer
                .Setup(x => x.GetSentimentAsync(
                           It.Is<IEnumerable<string>>(
                               words => words.SequenceEqual(redditResults.Select(redditResult => $"{redditResult.Title}. {redditResult.Content}")))))
                .ReturnsAsync(sentiments);

            // Act.
            var results = await analyzer.GetSentimentAsync(subreddit, searchTerm);

            // Assert.
            redditSearch.VerifyAll();
            sentimentAnalyzer.VerifyAll();

            results.ShouldNotBeNull();
            results.Posts.ShouldNotBeNull();
            results.Posts.ShouldNotBeEmpty();
            foreach (var result in results.Posts)
            {
                result.Title.ShouldNotBeNullOrWhiteSpace();
                result.Url.ShouldNotBeNullOrWhiteSpace();
            }
        }
    }
}