using System.Collections.Generic;
using Nest;
using Tipalti.TheWho.Documents;

namespace Tipalti.TheWho.Repository
{
    public interface IElasticSearchRepository
    {
        public void AddOrUpdate<TDocument>(TDocument document) where TDocument : class;
        public TDocument GetDocumentById<TDocument>(Id id) where TDocument : class;
        public List<ResourceDocument> GetResourceDocumentsByDomain(string domainId);
        public void DeleteDocument<TDocument>(Id id) where TDocument : class;
        public void BulkAddOrUpdate<TDocument>(List<TDocument> document) where TDocument : class;
    }
}
