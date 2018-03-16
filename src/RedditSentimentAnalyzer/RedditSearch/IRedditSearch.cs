using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedditSentimentAnalyzer.RedditSearch
{
    public interface IRedditSearch
    {
        Task<IEnumerable<SearchResult>> SearchAsync(string subreddit,
                                                    string searchTerm,
                                                    int limit = 100);
    }
}