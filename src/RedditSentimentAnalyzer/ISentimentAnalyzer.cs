using System.Threading.Tasks;
using RedditSentimentAnalyzer.Models;

namespace RedditSentimentAnalyzer
{
    public interface ISentimentAnalyzer
    {
        Task<SentimentAnalysis> GetSentimentAsync(string subreddit,
                                                  string searchTerm);
    }
}