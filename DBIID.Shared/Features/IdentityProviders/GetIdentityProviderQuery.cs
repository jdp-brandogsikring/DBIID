using DBIID.Application.Shared.Attributes;
using DBIID.Shared.Features.IdentityProviders;
using DBIID.Shared.Results;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Shared.Features.Users
{
    // GET Request
    [HttpRequest(HttpMethodType.GET, "User/Read/{id}")]
    public class GetIdentityProviderQuery : IRequest<Result<IdentityProviderDto>>
    {
        public int Id { get; set; }
    }

    public class GetIdentityProviderQueryValidator : AbstractValidator<GetIdentityProviderQuery>
    {
        public GetIdentityProviderQueryValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}
