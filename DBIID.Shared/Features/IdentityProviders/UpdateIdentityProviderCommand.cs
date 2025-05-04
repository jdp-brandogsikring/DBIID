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
    // PUT Request
    [HttpRequest(HttpMethodType.PUT, "IdentityProvider/Update/{id}")]
    public class UpdateIdentityProviderCommand : IRequest<Result<IdentityProviderDto>>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }


    public class UpdateIdentityProviderCommandValidator : AbstractValidator<UpdateIdentityProviderCommand>
    {
        public UpdateIdentityProviderCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
