using System;
using Elasticsearch.Net;
using Nest;

namespace Tipalti.TheWho.ElasticClient
{
    public sealed class ElasticSearchClient
    {
        public const string DefaultIndex = "recourse";
        private static readonly ElasticSearchClient Instance = new ElasticSearchClient();

        public Nest.ElasticClient ElasticClient { get; set; }

        static ElasticSearchClient()
        {
        }

        private ElasticSearchClient()
        {
            SetElasticClient();
        }

        public static ElasticSearchClient GetElasticSearchClient => Instance;

        private void SetElasticClient()
        {
            var singleNode = new SingleNodeConnectionPool(new Uri("https://the-who.es.us-central1.gcp.cloud.es.io:9243"));
            var settings = new ConnectionSettings(singleNode).BasicAuthentication("elastic", "pcw6SzZSjyMpecZUhaiwBbx2"); ;
            settings.DefaultIndex(DefaultIndex);
            ElasticClient = new Nest.ElasticClient(settings);
        }
    }
}
