using DBIID.Application.Common.Data;
using DBIID.Infrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Infrastructure.Data.Commmen
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MasterDbContext context;

        public UnitOfWork(MasterDbContext context)
        {
            this.context = context;
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }
    }
}
