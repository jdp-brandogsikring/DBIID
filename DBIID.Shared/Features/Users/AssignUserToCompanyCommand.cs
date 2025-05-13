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
    [HttpRequest(HttpMethodType.POST, "User/AssignToCompany")]
    public class AssignUserToCompanyCommand : IRequest<Result>
    {
        public int UserId { get; set; }
        public int CompanyId { get; set; }
    }

    public class AssignUserToCompanyCommandValidator : AbstractValidator<AssignUserToCompanyCommand>
    {
        public AssignUserToCompanyCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.CompanyId).NotEmpty();
        }
    }
}
