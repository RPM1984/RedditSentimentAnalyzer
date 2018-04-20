using Microsoft.Extensions.Logging;
using RedditSentimentAnalyzer.Analysis;
using RedditSentimentAnalyzer.RedditSearch;

namespace RedditSentimentAnalyzer.Tests.AzureSentimentAnalyzer
{
    public class FakeAzureSentimentAnalyzer : SentimentAnalyzerBase
    {
        public FakeAzureSentimentAnalyzer(ILogger logger,
                                          IAzureSentimentAnalyzer azureSentimentAnalyzer,
                                          IRedditSearch redditSearch) : base(logger, azureSentimentAnalyzer, redditSearch)
        {
        }
    }
}