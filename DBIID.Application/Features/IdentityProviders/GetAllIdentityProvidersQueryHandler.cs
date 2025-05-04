using AutoMapper;
using DBIID.Application.Common.Handlers;
using DBIID.Shared.Features.IdentityProviders;
using DBIID.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Application.Features.IdentityProviders
{
    public class GetAllIdentityProvidersQueryHandler : IQueryHandler<GetAllIdentityProvidersQuery, Result<IEnumerable<IdentityProviderDto>>>
    {
        private readonly IIdentityProviderRepository identityProviderRepository;
        private readonly IMapper mapper;

        public GetAllIdentityProvidersQueryHandler(IIdentityProviderRepository identityProviderRepository,
                                                   IMapper mapper)
        {
            this.identityProviderRepository = identityProviderRepository;
            this.mapper = mapper;
        }

        public async Task<Result<IEnumerable<IdentityProviderDto>>> Handle(GetAllIdentityProvidersQuery request, CancellationToken cancellationToken)
        {
            var identityProviders = identityProviderRepository.GetAll().ToList();

            return Result<IEnumerable<IdentityProviderDto>>.Success(identityProviders.Select(x => new IdentityProviderDto
            {
                Id = x.Id,
                Name = x.Name,
                Secret = x.Secret, 
                TenantId = x.TenantId,
            }).ToList());
        }
    }
}
