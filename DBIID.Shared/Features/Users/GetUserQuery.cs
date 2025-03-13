using DBIID.Application.Shared.Attributes;
using DBIID.Application.Shared.Dtos;
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
    // GET Request
    [HttpRequest(HttpMethodType.GET, "User/Read/{id}")]
    public class GetUserQuery : IRequest<Result<UserDto>>
    {
        public int Id { get; set; }
    }

    public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
    {
        public GetUserQueryValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}
