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

        public List<Documents.ResourceDocumentResult> GetResourceDocumentsByDomain(string domainId)
        {
            ISearchResponse<Documents.ResourceDocumentResult> searchResult = _elasticSearchClient.Search<Documents.ResourceDocumentResult>(s => s
                .Index(GetIndexName(typeof(Documents.ResourceDocumentResult)))
                .Query(q => q
                    .Match(m => m
                        .Field("domains.domainId")
                        .Query(domainId.ToString())
                    )
                )
            );
            return searchResult?.Documents?.ToList();
        }

        public List<ServiceDocument> GetServiceByOwner(string serviceName)
        {
            throw new NotImplementedException();
        }

        public void DeleteDocument<TDocument>(Id id) where TDocument : class
        {
            _elasticSearchClient.Delete<TDocument>(id);
        }

        public static string GetIndexName(Type type)
        {
            string indexName = ((IndexNameAttribute) Attribute.GetCustomAttribute(type, typeof(IndexNameAttribute)))?.IndexName;
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
            throw new NotImplementedException();
        }

        public SpacesDocument GetSpaces()
        {
            throw new NotImplementedException();
        }

        public TeamDocument GetTeamConfiguration(int domainId)
        {
            throw new NotImplementedException();
        }
    }
}
