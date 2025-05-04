using DBIID.Application.Shared.Attributes;
using DBIID.Shared.Features.IdentityProviders;
using DBIID.Shared.Features.Users;
using DBIID.Shared.Results;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Shared.Features.IdentityProviders
{
    [HttpRequest(HttpMethodType.POST, "IdentityProvider/Create")]
    public class CreateIdentityProviderCommand: IRequest<Result<IdentityProviderDto>>
    {
        public string Name { get; set; }
    }

    public class CreateIdentityProviderCommandValidator : AbstractValidator<CreateIdentityProviderCommand>
    {
        public CreateIdentityProviderCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
