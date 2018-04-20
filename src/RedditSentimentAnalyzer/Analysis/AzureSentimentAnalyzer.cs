using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using Microsoft.Extensions.Logging;

namespace RedditSentimentAnalyzer.Analysis
{
    public class AzureSentimentAnalyzer : IAzureSentimentAnalyzer
    {
        private readonly ILogger _logger;
        private readonly ITextAnalyticsApiWrapper _textAnalyticsApi;

        public AzureSentimentAnalyzer(ILogger logger,
                                      ITextAnalyticsApiWrapper textAnalyticsApi)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _textAnalyticsApi = textAnalyticsApi ?? throw new ArgumentNullException(nameof(textAnalyticsApi));
        }

        public AzureSentimentAnalyzer(ILogger logger,
                                      string subscriptionKey,
                                      AzureRegions region) : this(logger, new TextAnalyticsWrapper(subscriptionKey, region))
        {
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

            var results = await _textAnalyticsApi.SentimentAsync(inputs)
                                                 .ConfigureAwait(false);
            var sentimentResults = new Dictionary<string, double>();
            if (results?.Documents != null)
            {
                foreach (var result in results.Documents.Where(result => result.Score.HasValue))
                {
                    if (sentimentResults.ContainsKey(inputMaps[result.Id]) ||
                        !result.Score.HasValue)
                    {
                        continue;
                    }

                    sentimentResults.Add(inputMaps[result.Id], result.Score.Value);
                }
            }

            return sentimentResults;
        }
    }
}