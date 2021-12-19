using System.Threading.Tasks;
using Tipalti.Utils.Result;

namespace Tipalti.TheWho.Indexers
{
    public interface IServiceIndexer
    {
        Task<Result> RunAsync();
    }
}
