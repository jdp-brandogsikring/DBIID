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
    // PUT Request
    [HttpRequest(HttpMethodType.PUT, "User/Update/{id}")]
    public class UpdateUserCommand : IRequest<Result<UserDto>>
    {
        public int Id { get; set; }
        public string GivenName { get; set; } = string.Empty;
        public string FamilyName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }


    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.GivenName).NotEmpty();
            RuleFor(x => x.FamilyName).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}
