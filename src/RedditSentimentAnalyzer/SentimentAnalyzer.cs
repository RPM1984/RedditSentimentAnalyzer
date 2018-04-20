using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using Microsoft.Extensions.Logging;
using RedditSentimentAnalyzer.Analysis;

namespace RedditSentimentAnalyzer
{
    public class SentimentAnalyzer : SentimentAnalyzerBase
    {
        public SentimentAnalyzer(ILogger logger,
                                 string azureCognitiveServicesSubscriptionKey,
                                 AzureRegions azureCognitiveServicesRegion) : base(logger,
                                                                                   new AzureSentimentAnalyzer(
                                                                                       logger,
                                                                                       azureCognitiveServicesSubscriptionKey,
                                                                                       azureCognitiveServicesRegion),
                                                                                   new RedditSearch.RedditSearch(logger))
        {
        }
    }
}