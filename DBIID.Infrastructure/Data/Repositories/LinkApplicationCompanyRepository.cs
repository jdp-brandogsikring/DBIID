using DBIID.Application.Features.Companies;
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
    public class LinkApplicationCompanyRepository : GenericRepository<LinkApplicationCompany>, ILinkApplicationCompanyRepository
    {
        public LinkApplicationCompanyRepository(MasterDbContext context) : base(context)
        {
        }

        public IQueryable<LinkApplicationCompany> GetAllIncludeApplications()
        {
            return context.LinkApplicationCompanies
                .Include(x => x.Application);
        }
    }
    
}
