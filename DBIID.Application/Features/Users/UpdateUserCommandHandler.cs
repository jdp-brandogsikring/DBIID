﻿using AutoMapper;
using DBIID.Application.Common.Data;
using DBIID.Application.Common.Handlers;
using DBIID.Shared.Features.Users;
using DBIID.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Application.Features.Users
{
    public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, Result<UserDto>>
    {
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IUserSyncService userSyncService;

        public UpdateUserCommandHandler(IUserRepository userRepository,
                                        IUnitOfWork unitOfWork,
                                        IMapper mapper,
                                        IUserSyncService userSyncService)
        {
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.userSyncService = userSyncService;
        }

        public async Task<Result<UserDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = userRepository.GetById(request.Id);
            if (user == null)
            {
                return Result<UserDto>.Error("User not found");
            }

            var userWithEmail = userRepository.GetAll().FirstOrDefault(x => x.Email.ToLower() == request.Email.ToLower());
            if (userWithEmail != null && userWithEmail.Id != user.Id)
            {
                return Result<UserDto>.Error("Email already exists");
            }

            user.GivenName = request.GivenName;
            user.FamilyName = request.FamilyName;
            user.Email = request.Email;
            user.Phone = request.Phone;
            user.Modified = DateTime.UtcNow;

            await unitOfWork.SaveChangesAsync();

            await userSyncService.PushUserChangesToClients(user, UserSyncActionType.Update);

            return Result<UserDto>.Success(mapper.Map<UserDto>(user));
        }
    }
}
