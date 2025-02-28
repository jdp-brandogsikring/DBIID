
using DBIID.Application.Shared.Attributes;
using DBIID.Application.Shared.Dtos;
using DBIID.Shared.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Shared.Features.Users
{
    [HttpRequest(HttpMethodType.GET, "User/All")]
    public class GetAllUserQuery : IRequest<Result<IEnumerable<UserDto>>>
    {
    }
}
