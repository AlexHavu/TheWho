using System.Collections.Generic;
using Nest;
using Tipalti.TheWho.Dal.Elastic.Documents;

namespace Tipalti.TheWho.Dal.Elastic
{
    public interface IDbElasticTheWhoRepository
    {
        public void AddOrUpdate<TDocument>(TDocument document) where TDocument : class;
        public TDocument GetDocumentById<TDocument>(Id id) where TDocument : class;
        public List<ResourceDocument> GetResourceDocumentsByDomain(string domainId);
        public List<ServiceDocument> GetServiceByOwner(string serviceName);
        public List<TeamConfigurationDocument> GetTeamConfiguration();
        public List<string> GetDomains();
        public List<string> GetSpacesKeys();
        public TeamDocument GetTeamConfiguration(int domainId);
        public void DeleteDocument<TDocument>(Id id) where TDocument : class;
        public void BulkAddOrUpdate<TDocument>(List<TDocument> document) where TDocument : class;
        public Dictionary<string, TeamConfigurationDocument> GetTeams();
        public void DeleteIndex(string indexName);
        public void CreateSimpleIndex<TDocument>() where TDocument : class;
        public void CreateTeamIndex();
    }
}
