using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tipalti.Utils.Result;

namespace Tipalti.TheWho.Indexers
{
    public interface IConfluenceIndexer
    {
        Task<Result> RunAsync();
    }
}
