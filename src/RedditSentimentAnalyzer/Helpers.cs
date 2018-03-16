namespace RedditSentimentAnalyzer
{
    public static class Helpers
    {
        public static string ToDescription(this double sentimentValue)
        {
            if (sentimentValue == 0.5)
            {
                return "Neutral";
            }

            return sentimentValue > 0.5
                       ? "Positive"
                       : "Negative";
        }
    }
}