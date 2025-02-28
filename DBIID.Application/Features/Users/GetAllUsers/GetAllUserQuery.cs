using DBIID.Application.Common.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Application.Features.Users.GetAllUsers
{
    public class GetAllUserQuery : IRequest<IEnumerable<UserDto>>
    {
    }
}
