
using DBIID.Application.Shared.Attributes;
using DBIID.Application.Shared.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Shared.Features.Users
{
    [HttpRequest(HttpMethodType.GET, "User/All")]
    public class GetAllUserQuery : IRequest<IEnumerable<UserDto>>
    {
    }
}
