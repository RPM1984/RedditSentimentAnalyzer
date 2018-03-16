namespace RedditSentimentAnalyzer.Models
{
    public class RedditPost
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public double Sentiment { get; set; }
    }
}