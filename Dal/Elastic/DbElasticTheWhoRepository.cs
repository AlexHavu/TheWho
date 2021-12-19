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

        public List<ResourceDocumentResult> GetResourceDocumentsByDomain(int domainId)
        {
            ISearchResponse<ResourceDocumentResult> searchResult = _elasticSearchClient.Search<ResourceDocumentResult>(s => s
                .Index(GetIndexName(typeof(ResourceDocumentResult)))
                .Query(q => q
                    .Match(m => m
                        .Field("domains.domainId")
                        .Query(domainId.ToString())
                    )
                )
            );
            return searchResult?.Documents?.ToList();
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

        public void CreateRecourseIndexAndMapping()
        {
            var createIndexResponse = _elasticSearchClient.Indices.Create(GetIndexName(typeof(ResourceDocumentResult)), c => c
                .Map<ResourceDocumentResult>(m => m
                    .AutoMap()
                    .Properties(ps => ps
                        .Nested<DomainModel>(n => n
                            .Name(nn => nn.Domains)
                        )
                    )
                )
            );
        }
        public void DeleteAllDocuments()
        {
            var response = _elasticSearchClient.Indices.DeleteAsync(GetIndexName(typeof(TeamDocument)));
            response = _elasticSearchClient.Indices.DeleteAsync(GetIndexName(typeof(ResourceDocumentResult)));
        }

        public void CreateTeamIndexAndMapping()
        {
            var createIndexResponse = _elasticSearchClient.Indices.Create(GetIndexName(typeof(TeamDocument)), c => c
                .Map<TeamDocument>(m => m
                    .AutoMap()
                    .Properties(ps => ps
                        .Nested<TeamMemberModel>(n => n
                            .Name(nn => nn.TeamLeader)
                        )
                    )
                )
            );
        }

        //add async and error handling
        //============================
        //var indexManyResponse = client.IndexMany(people);
        //if (indexManyResponse.Errors)
        //{
        //    // the response can be inspected for errors
        //    foreach (var itemWithError in indexManyResponse.ItemsWithErrors)
        //    {
        //        // if there are errors, they can be enumerated and inspected
        //        Console.WriteLine("Failed to index document {0}: {1}",
        //            itemWithError.Id, itemWithError.Error);
        //    }
        //}
        //// alternatively, documents can be indexed asynchronously
        //var indexManyAsyncResponse = await client
        //public void CreateMapping()
        //{
        //    var createIndexResponse = _elasticSearchClient.Indices.Create("test11", c => c
        //        .Map<RecourseDocument>(m => m
        //            .Properties(ps => ps
        //                .Text(s => s
        //                    .Name(n => n.Title))
        //                    .Number(s => s
        //                        .Name(n => n.RecourseType))
        //                .Number(s => s
        //                    .Name(n => n.Link)
        //                )
        //                .Object<Employee>(o => o
        //                    .Name(n => n.Employees)
        //                    .Properties(eps => eps
        //                        .Text(s => s
        //                            .Name(e => e.FirstName)
        //                        )
        //                        .Text(s => s
        //                            .Name(e => e.LastName)
        //                        )
        //                        .Number(n => n
        //                            .Name(e => e.Salary)
        //                            .Type(NumberType.Integer)
        //                        )
        //                    )
        //                )
        //            )
        //        )
        //    );


        //var createIndexResponse = _elasticSearchClient.Indices.Create("recourse", c => c
        //    .Map<RecourseDocument>(m => m
        //        .AutoMap()
        //        .Properties(ps => ps
        //            .Nested<DomainModel>(n => n
        //                .Name(nn => nn.Domains)
        //            )
        //        )
        //    )
        //);
        //   }
    }
}
