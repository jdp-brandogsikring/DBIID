using DBIID.Application.Features.Applications;
using DBIID.Domain.Entities;
using DBIID.Infrastructure.Data.Commmen;
using DBIID.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Infrastructure.Data.Repositories
{
    public class ApplicationRepository : GenericRepository<Domain.Entities.Application>, IApplicationRepository
    {
        public ApplicationRepository(MasterDbContext context) : base(context)
        {
        }

        public IQueryable<Domain.Entities.Application> GetAllIncludeCompanies()
        {
            return context.Applications
                .Include(x => x.Links);
        }
    }
}
