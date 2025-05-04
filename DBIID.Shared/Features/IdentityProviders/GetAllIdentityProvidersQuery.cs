using DBIID.Application.Shared.Attributes;
using DBIID.Shared.Features.Users;
using DBIID.Shared.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Shared.Features.IdentityProviders
{

    [HttpRequest(HttpMethodType.GET, "IdentityProviders/All")]
    public class GetAllIdentityProvidersQuery : IRequest<Result<IEnumerable<IdentityProviderDto>>>
    {
    }
}
