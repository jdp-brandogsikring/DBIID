using DBIID.Application.Common.Data;
using DBIID.Application.Common.Handlers;
using DBIID.Shared.Features.IdentityProviders;
using DBIID.Shared.Features.Users;
using DBIID.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Application.Features.IdentityProviders
{
    public class UpdateIdentityProviderCommandHandler : ICommandHandler<UpdateIdentityProviderCommand, Result<IdentityProviderDto>>
    {
        private readonly IIdentityProviderRepository identityProviderRepository;
        private readonly IUnitOfWork unitOfWork;

        public UpdateIdentityProviderCommandHandler(IIdentityProviderRepository identityProviderRepository,
                                                    IUnitOfWork unitOfWork)
        {
            this.identityProviderRepository = identityProviderRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<IdentityProviderDto>> Handle(UpdateIdentityProviderCommand request, CancellationToken cancellationToken)
        {
            var identityProvider = identityProviderRepository.GetById(request.Id);
            if (identityProvider == null)
            {
                return Result<IdentityProviderDto>.Error("Identity provider not found");
            }
            var identityProviderWithName = identityProviderRepository.GetAll().FirstOrDefault(x => x.Name.ToLower() == request.Name.ToLower());
            if (identityProviderWithName != null && identityProviderWithName.Id != identityProvider.Id)
            {
                return Result<IdentityProviderDto>.Error("Identity provider name already exists");
            }
            identityProvider.Name = request.Name;
            identityProviderRepository.Update(identityProvider);
            await unitOfWork.SaveChangesAsync();
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
