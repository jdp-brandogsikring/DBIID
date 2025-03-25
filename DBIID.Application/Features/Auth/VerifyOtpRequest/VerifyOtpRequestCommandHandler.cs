using AutoMapper;
using DBIID.Application.Common.Data;
using DBIID.Application.Common.Handlers;
using DBIID.Application.Features.Users;
using DBIID.Shared.Features.Users;
using DBIID.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Application.Features.Auth.VerifyOtpRequest
{
    public class VerifyOtpRequestCommandHandler : ICommandHandler<VerifyOtpRequestCommand, Result<UserDto>>
    {
        private readonly IOtpTransactionRepository otpTransactionRepository;
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public VerifyOtpRequestCommandHandler(IOtpTransactionRepository otpTransactionRepository,
                                              IUserRepository userRepository,
                                              IUnitOfWork unitOfWork,
                                              IMapper mapper)
        {
            this.otpTransactionRepository = otpTransactionRepository;
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<Result<UserDto>> Handle(VerifyOtpRequestCommand request, CancellationToken cancellationToken)
        {
            var transaction = otpTransactionRepository.GetAll().Where(x =>
                                                                x.TransactionId == request.OtpTransactionId &&
                                                                x.UserId == request.UserId).FirstOrDefault(); 
                if (transaction == null)
            {
                return Result<UserDto>.Error("Invalid OTP Transaction ID");
            }

            if (transaction.Expire < DateTime.Now)
            {
                return Result<UserDto>.Error("OTP Transaction expired");
            }

            if (transaction.IsUsed)
            {
                return Result<UserDto>.Error("OTP Transaction already used");
            }

            if (transaction.OtpCode != request.OtpCode)
            {
                return Result<UserDto>.Error("Invalid OTP Code");
            }

            transaction.IsUsed = true;
            await unitOfWork.SaveChangesAsync();

            var user = userRepository.GetById(request.UserId);

            return Result<UserDto>.Success(mapper.Map<UserDto>(user));
        }
    }
}
