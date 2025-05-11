using DBIID.Application.Shared.Attributes;
using DBIID.Shared.Features.Companies;
using DBIID.Shared.Results;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Shared.Features.Applications
{
    [HttpRequest(HttpMethodType.POST, "Applications/Create")]
    public class CreateApplicationCommand : IRequest<Result<ApplicationDto>>
    {
        public string Name { get; set; } = string.Empty;
    }

    public class CreateApplicationCommandValidator : AbstractValidator<CreateApplicationCommand>
    {
        public CreateApplicationCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
