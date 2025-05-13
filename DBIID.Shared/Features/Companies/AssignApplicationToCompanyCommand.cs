using DBIID.Application.Shared.Attributes;
using DBIID.Shared.Results;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Shared.Features.Companies
{
    [HttpRequest(HttpMethodType.POST, "Company/AssignApplication")]
    public class AssignApplicationToCompanyCommand : IRequest<Result>
    {
        public int ApplicationId { get; set; }
        public int CompanyId { get; set; }
    }

    public class AssignApplicationToCompanyCommandValidator : AbstractValidator<AssignApplicationToCompanyCommand>
    {
        public AssignApplicationToCompanyCommandValidator()
        {
            RuleFor(x => x.ApplicationId).NotEmpty();
            RuleFor(x => x.CompanyId).NotEmpty();
        }
    }
}
