using DBIID.Application.Common.Handlers;
using DBIID.Shared.Features.IdentityProviders;
using DBIID.Shared.Features.Users;
using DBIID.Shared.Results;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Application.Features.IdentityProviders
{
    public class GetIdentityProviderQueryHandler : IQueryHandler<GetIdentityProviderQuery, Result<IdentityProviderDto>>
    {
        private readonly IIdentityProviderRepository identityProviderRepository;

        public GetIdentityProviderQueryHandler(IIdentityProviderRepository identityProviderRepository)
        {
            this.identityProviderRepository = identityProviderRepository;
        }        
        
        public async Task<Result<IdentityProviderDto>> Handle(GetIdentityProviderQuery request, CancellationToken cancellationToken)
        {
            var identityProvider = identityProviderRepository.GetById(request.Id);
            if (identityProvider == null)
            {
                return Result<IdentityProviderDto>.Error("Identity provider not found");
            }
            return Result<IdentityProviderDto>.Success(new IdentityProviderDto
            {
                Id = identityProvider.Id,
                Name = identityProvider.Name,
                Secret = identityProvider.Secret,
                TenantId = identityProvider.TenantId,
            });
        }
    }
}
