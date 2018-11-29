using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Hcs.Platform.Data
{
    public interface IScopedDbContext<TDbContext> where TDbContext : DbContext
    {
        TDbContext DbContext { get; }
        IDbContextTransaction Transaction { get; set; }
    }
    internal class ScopedDbContext<TDbContext> : IScopedDbContext<TDbContext>, IDisposable
     where TDbContext : DbContext
    {
        public IDbContextTransaction Transaction { get; set; }
        public ScopedDbContext(TDbContext dbContext)
        {
            DbContext = dbContext;
        }
        public TDbContext DbContext { get; }

        public void Dispose()
        {
            Transaction?.Dispose();
        }
    }
}
