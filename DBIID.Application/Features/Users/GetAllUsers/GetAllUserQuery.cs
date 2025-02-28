using DBIID.Application.Common.Attributes;
using DBIID.Application.Common.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Application.Features.Users.GetAllUsers
{
    [HttpRequest("GET", "User/All")]
    public class GetAllUserQuery : IRequest<IEnumerable<UserDto>>
    {
    }
}
