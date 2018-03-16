using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using Microsoft.Extensions.Logging;
using RedditSentimentAnalyzer.Analysis;
using RedditSentimentAnalyzer.Models;
using RedditSentimentAnalyzer.RedditSearch;

namespace RedditSentimentAnalyzer
{
    public class SentimentAnalyzer : ISentimentAnalyzer
    {
        private readonly ILogger _logger;
        private readonly IAzureSentimentAnalyzer _sentimentAnalyzer;
        private readonly IRedditSearch _redditSearch;

        // tests
        public SentimentAnalyzer(ILogger logger,
                                 IAzureSentimentAnalyzer azureSentimentAnalyzer,
                                 IRedditSearch redditSearch)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _sentimentAnalyzer = azureSentimentAnalyzer ?? throw new ArgumentNullException(nameof(azureSentimentAnalyzer));
            _redditSearch = redditSearch ?? throw new ArgumentNullException(nameof(azureSentimentAnalyzer));
        }

        public SentimentAnalyzer(ILogger logger,
                                 string azureCognitiveServicesSubscriptionKey,
                                 AzureRegions azureCognitiveServicesRegion)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            if (string.IsNullOrWhiteSpace(azureCognitiveServicesSubscriptionKey))
            {
                throw new ArgumentNullException(nameof(azureCognitiveServicesSubscriptionKey));
            }

            _sentimentAnalyzer = new AzureSentimentAnalyzer(logger, azureCognitiveServicesSubscriptionKey, azureCognitiveServicesRegion);
            _redditSearch = new RedditSearch.RedditSearch(logger);
        }

        public async Task<SentimentAnalysis> GetSentimentAsync(string subreddit,
                                                               string searchTerm)
        {
            _logger.LogTrace(nameof(GetSentimentAsync));

            if (string.IsNullOrWhiteSpace(subreddit))
            {
                throw new ArgumentNullException(nameof(subreddit));
            }

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                throw new ArgumentNullException(nameof(searchTerm));
            }

            _logger.LogDebug("Checking Reddit...");
            var redditResults = (await _redditSearch.SearchAsync(subreddit, searchTerm)
                                                    .ConfigureAwait(false)).ToList();
            if (!redditResults.Any())
            {
                return null;
            }

            var redditSentimentSearchKeyAndOriginalResponses =
                redditResults.ToDictionary(redditResult => $"{redditResult.Title}. {redditResult.Content}", redditResult => redditResult);

            var sentiments = await _sentimentAnalyzer.GetSentimentAsync(redditSentimentSearchKeyAndOriginalResponses.Keys)
                                                     .ConfigureAwait(false);
            if (sentiments != null)
            {
                return new SentimentAnalysis
                {
                    Posts = (from post in redditSentimentSearchKeyAndOriginalResponses
                             join r2 in sentiments on post.Key equals r2.Key
                             select new
                             {
                                 post,
                                 r2.Value
                             }
                            ).Select(sentiment => new RedditPost
                    {
                        Title = sentiment.post.Value.Title,
                        Url = $"https://www.reddit.com{sentiment.post.Value.Permalink}",
                        Sentiment = sentiment.Value
                    })
                };
            }

            return null;
        }
    }
}