using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tipalti.TheWho.Dal.Elastic;

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
            return _elasticDB.GetSearchResults(search);
        }
        public bool GetDocByResourceType(string type, int id)
        {
            return _elasticDB.GetSearchResults(type, id);
        }
    }
}
