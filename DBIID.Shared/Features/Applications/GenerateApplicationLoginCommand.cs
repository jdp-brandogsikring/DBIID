using DBIID.Application.Shared.Attributes;
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
    [HttpRequest(HttpMethodType.POST, "Applications/GenerateSSO/{Token}")]
    public class GenerateApplicationLoginCommand : IRequest<Result<ApplicationLoginDto>>
    {
        public string Token { get; set; } = string.Empty;
    }

    public class GenerateApplicationLoginCommandValidator : AbstractValidator<GenerateApplicationLoginCommand>
    {
        public GenerateApplicationLoginCommandValidator()
        {
            RuleFor(x => x.Token).NotEmpty();
        }
    }
}
