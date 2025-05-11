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

    [HttpRequest(HttpMethodType.POST, "Applications/Login/{Token}")]
    public class ApplicationLoginCommand : IRequest<Result<ApplicationLoginDto>>
    {
        public string Token { get; set; } = string.Empty;
    }

    public class ApplicationLoginCommandValidator : AbstractValidator<ApplicationLoginCommand>
    {
        public ApplicationLoginCommandValidator()
        {
            RuleFor(x => x.Token).NotEmpty();
        }
    }
}
