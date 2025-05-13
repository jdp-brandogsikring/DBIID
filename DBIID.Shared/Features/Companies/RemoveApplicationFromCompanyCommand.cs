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
    [HttpRequest(HttpMethodType.POST, "Company/RemoveApplication")]
    public class RemoveApplicationFromCompanyCommand : IRequest<Result>
    {
        public int ApplicationId { get; set; }
        public int CompanyId { get; set; }
    }

    public class RemoveApplicationFromCompanyCommandValidator : AbstractValidator<RemoveApplicationFromCompanyCommand>
    {
        public RemoveApplicationFromCompanyCommandValidator()
        {
            RuleFor(x => x.ApplicationId).NotEmpty();
            RuleFor(x => x.CompanyId).NotEmpty();
        }
    }
}
