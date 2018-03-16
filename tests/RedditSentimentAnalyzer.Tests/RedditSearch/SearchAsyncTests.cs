using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using Xunit;

namespace RedditSentimentAnalyzer.Tests.RedditSearch
{
    public class SearchAsyncTests
    {
        [Fact]
        public async Task GivenASubredditAndSearchTerm_SearchAsync_ReturnsResults()
        {
            // Arrange.
            const string subreddit = "cryptocurrency";
            const string searchTerm = "bitcoin";
            const int limit = 5;

            var logger = new Mock<ILogger<RedditSentimentAnalyzer.RedditSearch.RedditSearch>>();
            var search = new RedditSentimentAnalyzer.RedditSearch.RedditSearch(logger.Object);

            // Act.
            var results = await search.SearchAsync(subreddit, searchTerm, limit);

            // Assert.
            results.ShouldNotBeNull();
            results.ShouldNotBeEmpty();
            results.ShouldAllBe(result => !string.IsNullOrWhiteSpace(result.Title));
            results.ShouldContain(result => !string.IsNullOrWhiteSpace(result.Content));
        }
    }
}