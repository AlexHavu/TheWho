using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tipalti.TheWho.Models;

namespace Tipalti.TheWho.Services
{
     public interface ISearchService
     {
        public List<BaseSearchResult> SearchResults(string search);
     }
}
