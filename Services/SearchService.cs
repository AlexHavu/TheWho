using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tipalti.TheWho.Dal.Elastic;
using Tipalti.TheWho.Enums;

namespace Tipalti.TheWho.Services
{
    public class SearchService : ISearchService
    {
        private DbElasticTheWhoRepository _elasticDB;

        public SearchService(IDbElasticTheWhoRepository dbMongoTheWhoRepository )
        {
            _elasticDB = (DbElasticTheWhoRepository)dbMongoTheWhoRepository;
        }
        public bool SearchResults(string search)
        {
            var domainID = (int)Enum.Parse(typeof(eDomain), search);
            var resourceModel = _elasticDB.GetResourceDocumentsByDomain(domainID);
            return true;
        }
       
    }
}
