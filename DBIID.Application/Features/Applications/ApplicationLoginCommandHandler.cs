using DBIID.Application.Common.Handlers;
using DBIID.Shared.Features.Applications;
using DBIID.Shared.Results;
using MediatR;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Application.Features.Applications
{
    public class ApplicationLoginCommandHandler : ICommandHandler<ApplicationLoginCommand, Result<ApplicationLoginDto>>
    {
        CacheStorage cacheStorage = CacheStorage.Instance;

        public ApplicationLoginCommandHandler()
        {
            
        }

        public async Task<Result<ApplicationLoginDto>> Handle(ApplicationLoginCommand request, CancellationToken cancellationToken)
        {
            ApplicationLoginDto value = cacheStorage.Get<ApplicationLoginDto>(request.Token);
            if (value == null)
            {
                return Result<ApplicationLoginDto>.Error("Token not found");
            }
            return Result<ApplicationLoginDto>.Success(value);

        }
    }
}
