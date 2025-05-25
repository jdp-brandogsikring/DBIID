using DBIID.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Application.Features.Users
{
    public interface IUserSyncService
    {
        Task CompanyAddedToApplication(Company company, Domain.Entities.Application application);
        Task CompanyRemovedFromApplication(Company company, Domain.Entities.Application application);


        Task UserAddedToCompany(User user, Company company);
        Task UserRemovedCompany(User user, Company company);
        Task PushUserChangesToClients(User user, UserSyncActionType actionType);
    }

    public enum UserSyncActionType
    {
        Create,
        Update,
        Delete
    }
}
