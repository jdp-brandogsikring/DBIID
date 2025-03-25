using DBIID.Shared.Features.Login;
using DBIID.Shared.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Application.Features.Auth.Login
{
    public class LoginCommand : IRequest<Result<LoginResponse>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
