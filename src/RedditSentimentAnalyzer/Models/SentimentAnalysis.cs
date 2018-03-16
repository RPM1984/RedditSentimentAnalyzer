using System.Collections.Generic;
using System.Linq;

namespace RedditSentimentAnalyzer.Models
{
    public class SentimentAnalysis
    {
        public double Average => Posts.Average(sentiment => sentiment.Sentiment);
        public string Overall => Average.ToDescription();
        public IEnumerable<RedditPost> Posts { get; set; } = Enumerable.Empty<RedditPost>();
    }
}