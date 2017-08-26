using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMC.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        private UMCDbContext dbContext;

        public UMCDbContext Init()
        {
            return dbContext ?? (dbContext = new UMCDbContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}
