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
    [HttpRequest(HttpMethodType.PUT, "User/{UserId}/ResetPassword")]

    public class ResetPasswordCommand : IRequest<Result>
    {
        public int UserId { get; set; }
        public string Password { get; set; } = string.Empty;
    }


    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0);
            RuleFor(x => x.Password).NotEmpty();

        }
    }
}
