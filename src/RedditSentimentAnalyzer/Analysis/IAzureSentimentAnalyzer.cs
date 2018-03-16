using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedditSentimentAnalyzer.Analysis
{
    public interface IAzureSentimentAnalyzer
    {
        Task<Dictionary<string, double>> GetSentimentAsync(IEnumerable<string> content);
    }
}