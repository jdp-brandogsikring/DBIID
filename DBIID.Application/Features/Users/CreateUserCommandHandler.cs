using AutoMapper;
using DBIID.Application.Common.Data;
using DBIID.Application.Common.Handlers;
using DBIID.Application.Shared.Dtos;
using DBIID.Domain.Entities;
using DBIID.Shared.Features.Users;
using DBIID.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Application.Features.Users
{
    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Result<UserDto>>
    {
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public CreateUserCommandHandler(IUserRepository userRepository,
                                        IUnitOfWork unitOfWork,
                                        IMapper mapper)
        {
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<Result<UserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                GivenName = request.GivenName,
                FamilyName = request.FamilyName,
                Email = request.Email,
                Password = ""
            };

            await userRepository.AddAsync(user);
            await unitOfWork.SaveChangesAsync();
            return Result<UserDto>.Success(mapper.Map<UserDto>(user));
        }
    }
}
