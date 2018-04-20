using System;
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

        [Fact]
        public async Task GivenNoSubreddit_SearchAsync_ThrowsExpectedException()
        {
            // Arrange.
            var logger = new Mock<ILogger<RedditSentimentAnalyzer.RedditSearch.RedditSearch>>();
            var search = new RedditSentimentAnalyzer.RedditSearch.RedditSearch(logger.Object);

            // Act.
            var exception = await Should.ThrowAsync<ArgumentNullException>(search.SearchAsync("", "searchTerm"));

            // Assert.
            exception.ShouldNotBeNull();
            exception.ParamName.ShouldBe("subreddit");
        }

        [Fact]
        public async Task GivenNoSearchTerm_SearchAsync_ThrowsExpectedException()
        {
            // Arrange.
            var logger = new Mock<ILogger<RedditSentimentAnalyzer.RedditSearch.RedditSearch>>();
            var search = new RedditSentimentAnalyzer.RedditSearch.RedditSearch(logger.Object);

            // Act.
            var exception = await Should.ThrowAsync<ArgumentNullException>(search.SearchAsync("subreddit", ""));

            // Assert.
            exception.ShouldNotBeNull();
            exception.ParamName.ShouldBe("searchTerm");
        }

        [Fact]
        public async Task GivenAnInvalidLimit_SearchAsync_ThrowsExpectedException()
        {
            // Arrange.
            var logger = new Mock<ILogger<RedditSentimentAnalyzer.RedditSearch.RedditSearch>>();
            var search = new RedditSentimentAnalyzer.RedditSearch.RedditSearch(logger.Object);

            // Act.
            var exception = await Should.ThrowAsync<ArgumentOutOfRangeException>(search.SearchAsync("subreddit", "searchTerm", 101));

            // Assert.
            exception.ShouldNotBeNull();
            exception.ParamName.ShouldBe("limit");
        }
    }
}