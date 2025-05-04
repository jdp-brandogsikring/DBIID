using DBIID.Application.Common.Data;
using DBIID.Application.Common.Handlers;
using DBIID.Application.Features.IdentityProviders;
using DBIID.Domain.Entities;
using DBIID.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DBIID.Shared.Features.IdentityProviders
{
    public class CreateIdentityProviderCommandHandler : ICommandHandler<CreateIdentityProviderCommand, Result<IdentityProviderDto>>
    {
        private readonly IIdentityProviderRepository identityProviderRepository;
        private readonly IUnitOfWork unitOfWork;

        public CreateIdentityProviderCommandHandler(IIdentityProviderRepository identityProviderRepository,
                                                    IUnitOfWork unitOfWork)
        {
            this.identityProviderRepository = identityProviderRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<IdentityProviderDto>> Handle(CreateIdentityProviderCommand request, CancellationToken cancellationToken)
        {

            var identityProviderWithName = identityProviderRepository.GetAll().FirstOrDefault(x => x.Name.ToLower() == request.Name.ToLower());
            if (identityProviderWithName != null)
            {
                return Result<IdentityProviderDto>.Error("Identity provider name already exists");
            }

            var identityProvider = new IdentityProvider
            {
                Name = request.Name,
                Secret = Guid.NewGuid().ToString().Replace("-",""), // Generate a new secret
                TenantId = Guid.NewGuid().ToString().Replace("-", ""), // Generate a new tenant ID
            };

            await identityProviderRepository.AddAsync(identityProvider);
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
