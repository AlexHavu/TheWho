using System.Collections.Generic;
using Nest;
using Tipalti.TheWho.Dal.Elastic.Documents;

namespace Tipalti.TheWho.Dal.Elastic
{
    public interface IDbElasticTheWhoRepository
    {
        public void AddOrUpdate<TDocument>(TDocument document) where TDocument : class;
        public TDocument GetDocumentById<TDocument>(Id id) where TDocument : class;
        public List<ResourceDocumentResult> GetResourceDocumentsByDomain(int domainId);
        public void DeleteDocument<TDocument>(Id id) where TDocument : class;
        public void BulkAddOrUpdate<TDocument>(List<TDocument> document) where TDocument : class;
    }
}
