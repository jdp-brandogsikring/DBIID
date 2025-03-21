using DBIID.Application.Common.Data;
using DBIID.Application.Common.Handlers;
using DBIID.Application.Features.Auth;
using DBIID.Shared.Features.Users;
using DBIID.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Application.Features.Users
{
    public class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand, Result>
    {
        private readonly IUserRepository userRepository;
        private readonly IPasswordService passwordService;
        private readonly IUnitOfWork unitOfWork;

        public ResetPasswordCommandHandler(IUserRepository userRepository,
                                           IPasswordService passwordService,
                                           IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.passwordService = passwordService;
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = userRepository.GetById(request.UserId);
            if (user == null)
            {
                return Result.Error("User not found");
            }
            user.Password = passwordService.HashPassword(request.Password);
            await unitOfWork.SaveChangesAsync();
            return Result.Success("Adgangskoden er nulstillet");
        }
    }
}
