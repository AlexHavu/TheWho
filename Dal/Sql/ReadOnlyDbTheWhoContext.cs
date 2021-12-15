using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Tipalti.TheWho.Dal.Sql
{
    public interface IReadOnlyDbTheWhoContext : IDbTheWhoContext, IDisposable
    {

    }

    public class ReadOnlyDbTheWhoContext : DbTheWhoContext, IReadOnlyDbTheWhoContext
    {
        private readonly string _readOnlyContextDoesNotSupportDbChanges = "This is a read only db context. There are some capabilities this context lack, such as saving the context.";

        public ReadOnlyDbTheWhoContext(DbContextOptions<DbTheWhoContext> options) : base(options)
        { }

        public override int SaveChanges()
        {
            throw new NotSupportedException(_readOnlyContextDoesNotSupportDbChanges);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            throw new NotSupportedException(_readOnlyContextDoesNotSupportDbChanges);
        }
    }
}
