using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tipalti.Utils.Result;

namespace Tipalti.TheWho.Indexers
{
    public interface ITeamIndexer
    {
        Task<Result> RunAsync(string root);
    }
}
