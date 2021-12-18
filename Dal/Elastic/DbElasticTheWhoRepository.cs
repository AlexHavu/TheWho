using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tipalti.TheWho.Dal.Elastic
{
    public interface IDbElasticTheWhoRepository
    {
        public bool GetSearchResults(string search);
    }
    public class DbElasticTheWhoRepository : IDbElasticTheWhoRepository
    {

        public DbElasticTheWhoRepository()
        {
           
        }

        internal bool GetSearchResults(string type, int id)
        {
            throw new NotImplementedException();
        }

        public bool GetSearchResults(string search)
        {
            return true;  
        }
    }
}
