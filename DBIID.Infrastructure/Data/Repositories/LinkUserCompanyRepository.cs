using DBIID.Application.Features.Users;
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
    public class LinkUserCompanyRepository : GenericRepository<LinkUserCompany>, ILinkUserCompanyRepository
    {
        public LinkUserCompanyRepository(MasterDbContext context) : base(context)
        {
        }

        public IQueryable<LinkUserCompany> GetAllIncludeCompanies()
        {
            return context.LinkUserCompanies
                .Include(x => x.Company)
                .Include(x => x.User);
        }
    }
}
