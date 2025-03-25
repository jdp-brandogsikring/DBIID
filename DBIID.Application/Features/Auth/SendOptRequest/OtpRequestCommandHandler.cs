using DBIID.Application.Common.Data;
using DBIID.Application.Common.Handlers;
using DBIID.Application.Features.Users;
using DBIID.Domain.Entities;
using DBIID.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Application.Features.Auth.SendOptRequest
{
    public class OtpRequestCommandHandler : ICommandHandler<OtpRequestCommand, Result>
    {
        private readonly IOtpTransactionRepository otpTransactionRepository;
        private readonly IUserRepository userRepository;
        private readonly ISendOTPService sendOTPService;
        private readonly IUnitOfWork unitOfWork;

        public OtpRequestCommandHandler(IOtpTransactionRepository otpTransactionRepository,
                                        IUserRepository userRepository,
                                        ISendOTPService sendOTPService,
                                        IUnitOfWork unitOfWork)
        {
            this.otpTransactionRepository = otpTransactionRepository;
            this.userRepository = userRepository;
            this.sendOTPService = sendOTPService;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(OtpRequestCommand request, CancellationToken cancellationToken)
        {
            OtpTransaction transaction = otpTransactionRepository.GetAll().Where(x =>
                                                                            x.TransactionId == request.OtpTransactionId &&
                                                                            x.UserId == request.UserId).FirstOrDefault();
            if (transaction == null)
            {
                return Result.Error("Invalid OTP Transaction ID");
            }

            if (transaction.Expire < DateTime.Now)
            {
                return Result.Error("OTP Transaction expired");
            }

            if (transaction.IsUsed)
            {
                return Result.Error("OTP Transaction already used");
            }

            User user = userRepository.GetById(request.UserId);

            if(user == null) {
                return Result.Error("User not found");
            }

            switch (request.ContactMethodId.ToLower())
            {
                case "email":
                    transaction.Type = Domain.Enums.OtpTransactionType.EmailVerification;
                    await SendOtpEmail(transaction, user);
                    break;
                case "phone":
                    transaction.Type = Domain.Enums.OtpTransactionType.PhoneVerification;
                    await SendOtpPhone(transaction, user);
                    break;
                default:
                    return Result.Error("Invalid contact method");
            }

            await unitOfWork.SaveChangesAsync();
            return Result.Success("OTP Sendt");

        }

        private async Task SendOtpPhone(OtpTransaction transaction, User user)
        {
            await sendOTPService.SendOtpPhone(transaction.OtpCode, "+4520650166");
        }

        private async Task SendOtpEmail(OtpTransaction transaction, User user)
        {
            await sendOTPService.SendOtpEmail(transaction.OtpCode, user.Email);
        }
    }
}
