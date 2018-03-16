namespace RedditSentimentAnalyzer.RedditSearch
{
    public class SearchResponse
    {
        public SearchResponseData Data { get; set; }
    }

    public class SearchResponseData
    {
        public SearchResponseChild[] Children { get; set; }
    }

    public class SearchResponseChild
    {
        public SearchResponseChildData Data { get; set; }
    }

    public class SearchResponseChildData
    {
        public string Title { get; set; }
        public string SelfText { get; set; }
        public string Permalink { get; set; }
    }
}