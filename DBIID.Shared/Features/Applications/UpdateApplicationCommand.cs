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
    [HttpRequest(HttpMethodType.PUT, "Applications/{Id}")]
    public class UpdateApplicationCommand : IRequest<Result<ApplicationDto>>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string PushUrl { get; set; } = string.Empty;
        public bool EnablePush { get; set; } = false;
    }

    public class UpdateApplicationCommandValidator : AbstractValidator<UpdateApplicationCommand>
    {
        public UpdateApplicationCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Url).NotEmpty();
        }
    }
}
