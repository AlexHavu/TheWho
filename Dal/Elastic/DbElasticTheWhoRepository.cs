using System;
using System.Collections.Generic;
using System.Linq;
using Nest;
using Tipalti.TheWho.Dal.Elastic;
using Tipalti.TheWho.Dal.Elastic.Documents;
using static System.String;

namespace Tipalti.TheWho.Dal.Elastic
{
    public class DbElasticTheWhoRepository : IDbElasticTheWhoRepository
    {
        readonly Nest.ElasticClient _elasticSearchClient;

        public DbElasticTheWhoRepository()
        {
            _elasticSearchClient = ElasticSearchClient.GetElasticSearchClient.ElasticClient;
        }

        public void AddOrUpdate<TDocument>(TDocument document) where TDocument : class
        {
            IndexResponse response = _elasticSearchClient.Index(document,
                i => i.Index(GetIndexName(typeof(TDocument))));
        }

        public void BulkAddOrUpdate<TDocument>(List<TDocument> document) where TDocument : class
        {
            BulkResponse response = _elasticSearchClient.IndexMany(document, GetIndexName(typeof(TDocument)));
            if (response.Errors)
            {
                //replace with LOG
                Console.WriteLine($"Error in BulkAddOrUpdate for documents:" +
                                  $"{Join(',', response.ItemsWithErrors.Select(x => x.Id).ToList())}");
            }
        }

        public TDocument GetDocumentById<TDocument>(Id id) where TDocument : class
        {
            GetResponse<TDocument> getResponse = _elasticSearchClient.Get<TDocument>(id, x => x
                .Index(GetIndexName(typeof(TDocument))));
            return getResponse?.Source;
        }

        public List<ResourceDocument> GetResourceDocumentsByDomain(string domain)
        {

            var searchResults = _elasticSearchClient.Search<ResourceDocument>(s => s
                .Index(GetIndexName(typeof(ResourceDocument)))
                .Query(q => q.QueryString(qs => qs.Query(domain)))
                .Sort(st => st.Descending("_score"))
                .From(0)
                .Size(500)
            );
            return searchResults?.Documents?.ToList();
        }

        public List<ServiceDocument> GetServiceByOwner(string owner)
        {
           var searchResults = _elasticSearchClient.Search<ServiceDocument>(s => s
                .Index(GetIndexName(typeof(ServiceDocument)))
                .Query(q => q.QueryString(qs => qs.Query(owner)))
                .From(0)
                .Size(500)
            );
            return searchResults?.Documents?.ToList();
        }

        public void DeleteDocument<TDocument>(Id id) where TDocument : class
        {
            _elasticSearchClient.Delete<TDocument>(id);
        }

        public static string GetIndexName(Type type)
        {
            string indexName = ((IndexNameAttribute)Attribute.GetCustomAttribute(type, typeof(IndexNameAttribute)))?.IndexName;
            return indexName ?? ElasticSearchClient.DefaultIndex;
        }

        public void DeleteIndex(string indexName)
        {
            var response = _elasticSearchClient.Indices.DeleteAsync(indexName);
        }

        public void CreateTeamIndex()
        {
            var createIndexResponse = _elasticSearchClient.Indices.Create(GetIndexName(typeof(Documents.TeamDocument)), c => c
                .Map<Documents.TeamDocument>(m => m
                    .AutoMap()
                    .Properties(ps => ps
                        .Nested<Documents.TeamMemberModel>(n => n
                            .Name(nn => nn.TeamLeader)
                        )
                    )
                )
            );
        }

        public void CreateSimpleIndex<TDocument>() where TDocument : class
        {
            var createIndexResponse = _elasticSearchClient.Indices.Create(GetIndexName(typeof(TDocument)), c => c
                .Map<TDocument>(m => m
                    .AutoMap()
                )
            );
        }

        public Dictionary<string, TeamConfigurationDocument> GetTeams()
        {
            ISearchResponse<TeamConfigurationDocument> searchResult = _elasticSearchClient.Search<TeamConfigurationDocument>(s => s
                            .Index(GetIndexName(typeof(TeamConfigurationDocument)))
                            .Query(q => q)
                        );
            return searchResult?.Documents?.ToDictionary(x => x.TeamName);
        }

        public List<ServiceDocument> GetResourceServiceByName(string serviceName)
        {
            throw new NotImplementedException();
        }

        public List<TeamConfigurationDocument> GetTeamConfiguration()
        {
            throw new NotImplementedException();
        }

        public List<string> GetDomains()
        {
            List<string> domains = new List<string>();
            foreach (var document in GetAllDocuments<TeamConfigurationDocument>())
            {
                domains.AddRange(document.Domains);
            }

            return domains;
        }

        public TeamDocument GetTeamConfiguration(int domainId)
        {
            throw new NotImplementedException();
        }

        public List<string> GetSpacesKeys()
        {
            return GetDocumentById<SpacesDocument>(1)?.Spaces;
        }

        private List<TDocument> GetAllDocuments<TDocument>() where TDocument : class
        {
            var searchResponse = _elasticSearchClient.Search<TDocument>(s => s
                .Index(GetIndexName(typeof(TDocument)))
                .Query(q => q.MatchAll()
                )
            );

            return searchResponse.Documents.ToList();
        }
    }
}
