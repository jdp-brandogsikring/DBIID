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
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(MasterDbContext context) : base(context)
        {
        }

        public IQueryable<User> GetAllIncludeCompanies()
        {
            return context.Users
                .Include(x => x.Links);
        }
    }
}
