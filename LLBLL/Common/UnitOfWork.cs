using System;
using System.Threading.Tasks;

namespace LLBLL.Common
{
    public class UnitOfWork : IDisposable
    {
        private readonly LLDbContext dbContext;
        private bool disposed = false;
        public UnitOfWork(LLDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public bool SaveChage()
        {
            return dbContext.SaveChanges() > 0;
        }

        public async Task<bool> SaveChageAsync()
        {
            return await dbContext.SaveChangesAsync() > 0;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">The disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // dispose the db context.
                    dbContext.Dispose();
                }
            }
            disposed = true;
        }
    }
}
