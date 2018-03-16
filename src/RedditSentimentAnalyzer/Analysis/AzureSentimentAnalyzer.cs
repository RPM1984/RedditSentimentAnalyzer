using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using Microsoft.Extensions.Logging;

namespace RedditSentimentAnalyzer.Analysis
{
    public class AzureSentimentAnalyzer : IAzureSentimentAnalyzer
    {
        private readonly TextAnalyticsAPI _client;
        private readonly ILogger _logger;

        public AzureSentimentAnalyzer(ILogger logger,
                                      string subscriptionKey,
                                      AzureRegions region)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (string.IsNullOrWhiteSpace(subscriptionKey))
            {
                throw new ArgumentNullException(nameof(subscriptionKey));
            }

            _client = new TextAnalyticsAPI
            {
                AzureRegion = region,
                SubscriptionKey = subscriptionKey
            };
        }

        public async Task<Dictionary<string, double>> GetSentimentAsync(IEnumerable<string> content)
        {
            _logger.LogTrace(nameof(GetSentimentAsync));

            var inputMaps = new Dictionary<string, string>();
            var inputs = new List<MultiLanguageInput>();
            var counter = 0;
            foreach (var c in content)
            {
                counter++;

                inputMaps.Add(counter.ToString(), c);
                inputs.Add(new MultiLanguageInput("en", counter.ToString(), c));
            }

            _logger.LogDebug($"Calling Azure Text Analytics with {counter} words..");

            var results = await _client.SentimentAsync(new MultiLanguageBatchInput(inputs))
                                       .ConfigureAwait(false);
            var sentimentResults = new Dictionary<string, double>();
            foreach (var result in results.Documents.Where(result => result.Score.HasValue))
            {
                if (sentimentResults.ContainsKey(inputMaps[result.Id]) ||
                    !result.Score.HasValue)
                {
                    continue;
                }

                sentimentResults.Add(inputMaps[result.Id], result.Score.Value);
            }

            return sentimentResults;
        }
    }
}