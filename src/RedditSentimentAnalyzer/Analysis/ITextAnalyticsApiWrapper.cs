using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;

namespace RedditSentimentAnalyzer.Analysis
{
    public interface ITextAnalyticsApiWrapper
    {
        Task<SentimentBatchResult> SentimentAsync(IList<MultiLanguageInput> inputs);
    }
}