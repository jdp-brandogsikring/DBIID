using DBIID.Application.Features.Auth;
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
    public class OtpTransactionRepository : GenericRepository<OtpTransaction>, IOtpTransactionRepository
    {
        public OtpTransactionRepository(MasterDbContext context) : base(context)
        {
        }
    }
}
