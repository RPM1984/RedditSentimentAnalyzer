using Microsoft.Rest;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace RedditSentimentAnalyzer.Analysis
{
    public class ApiKeyServiceClientCredentials : ServiceClientCredentials
    {
        private readonly string _subscriptionKey;

        public ApiKeyServiceClientCredentials(string subscriptionKey)
        {
            if (string.IsNullOrWhiteSpace(subscriptionKey))
            {
                throw new System.ArgumentException(nameof(subscriptionKey));
            }

            _subscriptionKey = subscriptionKey;
        }

        public override Task ProcessHttpRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);
            return base.ProcessHttpRequestAsync(request, cancellationToken);
        }
    }
}