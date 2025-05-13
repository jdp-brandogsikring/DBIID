using DBIID.Application.Shared.Attributes;
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

    [HttpRequest(HttpMethodType.POST, "User/RemoveFromCompany")]
    public class RemoveUserFromCompanyCommand : IRequest<Result>
    {
        public int UserId { get; set; }
        public int CompanyId { get; set; }
    }

    public class RemoveUserFromCompanyCommandValidator : AbstractValidator<RemoveUserFromCompanyCommand>
    {
        public RemoveUserFromCompanyCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.CompanyId).NotEmpty();
        }
    }
}
