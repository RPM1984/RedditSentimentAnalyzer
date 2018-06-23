using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;

namespace RedditSentimentAnalyzer.Analysis
{
    // Moq doesn't support mocking extension meethods (`_client.SentimentAsync`), so we need to mock this class instead.
    public class TextAnalyticsWrapper : ITextAnalyticsApiWrapper
    {
        private readonly ITextAnalyticsAPI _client;

        public TextAnalyticsWrapper(string subscriptionKey, AzureRegions region)
        {
            if (string.IsNullOrWhiteSpace(subscriptionKey))
            {
                throw new ArgumentNullException(nameof(subscriptionKey));
            }

            _client = _client = new TextAnalyticsAPI(new ApiKeyServiceClientCredentials(subscriptionKey));
            _client.AzureRegion = region;
        }

        public Task<SentimentBatchResult> SentimentAsync(IList<MultiLanguageInput> inputs,
                                                         CancellationToken cancellationToken = default(CancellationToken))
        {
            return _client.SentimentAsync(new MultiLanguageBatchInput(inputs), cancellationToken);
        }
    }
}