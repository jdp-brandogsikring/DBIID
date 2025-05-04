using DBIID.Application.Features.IdentityProviders;
using DBIID.Domain.Entities;
using DBIID.Infrastructure.Data.Commmen;
using DBIID.Infrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Infrastructure.Data.Repositories
{
    public class IdentityProviderRepository : GenericRepository<IdentityProvider>, IIdentityProviderRepository
    {
        public IdentityProviderRepository(MasterDbContext context) : base(context)
        {
        }
    }
}
