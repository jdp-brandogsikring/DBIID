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
        public string Url { get; set; } = string.Empty;
        public string PushUrl { get; set; } = string.Empty;
        public bool EnablePush { get; set; } = false;
    }

    public class CreateApplicationCommandValidator : AbstractValidator<CreateApplicationCommand>
    {
        public CreateApplicationCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Url).NotEmpty();
        }
    }
}
