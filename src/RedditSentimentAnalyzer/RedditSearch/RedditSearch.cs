using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace RedditSentimentAnalyzer.RedditSearch
{
    public class RedditSearch : IRedditSearch
    {
        private static readonly HttpClient HttpClient = new HttpClient
        {
            BaseAddress = new Uri("https://www.reddit.com")
        };

        private readonly ILogger _logger;

        public RedditSearch(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<SearchResult>> SearchAsync(string subreddit,
                                                                 string searchTerm,
                                                                 int limit = 100)
        {
            _logger.LogTrace(nameof(SearchAsync));

            if (string.IsNullOrWhiteSpace(subreddit))
            {
                throw new ArgumentNullException(nameof(subreddit));
            }

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                throw new ArgumentNullException(nameof(searchTerm));
            }

            if (limit > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(limit));
            }

            var path = $"/r/{subreddit}/search.json?q={searchTerm}&restrict_sr=true&limit={limit}";
            _logger.LogDebug($"Calling Reddit API: {path}");
            var response = await HttpClient.GetStringAsync(path)
                                           .ConfigureAwait(false);

            var result = JsonConvert.DeserializeObject<SearchResponse>(response);
            return result?.Data.Children.Select(child =>
                                                    new SearchResult
                                                    {
                                                        Title = child.Data.Title,
                                                        Content = child.Data.SelfText,
                                                        Permalink = child.Data.Permalink
                                                    });
        }
    }
}