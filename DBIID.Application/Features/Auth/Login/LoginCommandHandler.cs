using DBIID.Application.Common.Data;
using DBIID.Application.Common.Handlers;
using DBIID.Application.Features.Users;
using DBIID.Domain.Entities;
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
    public class LoginCommandHandler : ICommandHandler<LoginCommand, Result<LoginResponse>>
    {
        private readonly IUserRepository userRepository;
        private readonly IOtpTransactionRepository otpTransactionRepository;
        private readonly IPasswordService passwordService;
        private readonly IOtpService otpService;
        private readonly IUnitOfWork unitOfWork;

        public LoginCommandHandler(IUserRepository userRepository,
                                   IOtpTransactionRepository otpTransactionRepository,
                                   IPasswordService passwordService,
                                   IOtpService otpService,
                                   IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.otpTransactionRepository = otpTransactionRepository;
            this.passwordService = passwordService;
            this.otpService = otpService;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var users = userRepository.GetAll().Where(x => x.Email.ToLower() == request.Email.ToLower());
            if(users.Count() == 0)
            {
                return Result<LoginResponse>.Error("User not found");
            }

            if(users.Count() != 1)
            {
                return Result<LoginResponse>.Error("Multiple users found");
            }

            var user = users.First();

            if (!passwordService.ValidatePassword(request.Password, user.Password))
            {
                return Result<LoginResponse>.Error("Invalid password");
            }

            OtpTransaction otpTransaction = new OtpTransaction
            {
                UserId = user.Id,
                OtpCode = otpService.GenerateOtp(),
                Expire = DateTime.Now.AddMinutes(5),
                IsUsed = false,
                TransactionId = Guid.NewGuid(),
                Type = Domain.Enums.OtpTransactionType.NotSet,
            };

            await otpTransactionRepository.AddAsync(otpTransaction);
            await unitOfWork.SaveChangesAsync();

            var loginresult = new LoginResponse
            {
                UserId = user.Id,
                OtpTransactionId = otpTransaction.TransactionId,
            };


            var email = user.Email;
            var maskedEmail = string.Format("{0}****{1}", email[0], email.Substring(email.IndexOf('@') - 1));

            loginresult.Types.Add(new ContactMethodDto()
            {
                Id = "EMAIL",
                Value = maskedEmail,
                Type = "EMAIL",
                Description = "Arbejde",
            });

            var maskedPhone = "20650166".Substring(0, 2) + "****" + "20650166".Substring(6, 2);
            loginresult.Types.Add(new ContactMethodDto()
            {
                Id = "PHONE",
                Value = maskedPhone,
                Type = "PHONE",
                Description = "Privat",
            });


            return Result<LoginResponse>.Success(loginresult);
        }
    }
}
