using Microsoft.EntityFrameworkCore;

namespace Tipalti.TheWho.Dal.Sql
{
    public interface IDbTheWhoContext
    {

    }
    public class DbTheWhoContext : DbContext, IDbTheWhoContext
    {
        public DbTheWhoContext(DbContextOptions<DbTheWhoContext> options) : base(options)
        {

        }
    }
}
