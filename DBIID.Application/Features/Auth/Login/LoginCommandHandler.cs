using DBIID.Application.Common.Handlers;
using DBIID.Application.Features.Users;
using DBIID.Shared.Dtos;
using DBIID.Shared.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Application.Features.Auth.Login
{
    public class LoginCommandHandler : ICommandHandler<LoginCommand, Result<LoginResult>>
    {
        private readonly IUserRepository userRepository;
        private readonly IPasswordService passwordService;

        public LoginCommandHandler(IUserRepository userRepository,
                                   IPasswordService passwordService)
        {
            this.userRepository = userRepository;
            this.passwordService = passwordService;
        }

        public async Task<Result<LoginResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var users = userRepository.GetAll().Where(x => x.Email.ToLower() == request.Email.ToLower());
            if(users.Count() == 0)
            {
                return Result<LoginResult>.Error("User not found");
            }

            if(users.Count() != 1)
            {
                return Result<LoginResult>.Error("Multiple users found");
            }

            var user = users.First();

            if (!passwordService.ValidatePassword(request.Password, user.Password))
            {
                return Result<LoginResult>.Error("Invalid password");
            }

            return Result<LoginResult>.Success(new LoginResult
            {
            
            });
        }
    }
}
