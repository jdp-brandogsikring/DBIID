using AutoMapper;
using DBIID.Application.Common.Handlers;
using DBIID.Application.Shared.Dtos;
using DBIID.Shared.Features.Users;
using DBIID.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Application.Features.Users
{
    class GetUserQueryHandler : IQueryHandler<GetUserQuery, Result<UserDto>>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public GetUserQueryHandler(IUserRepository userRepository,
                                    IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }
        public async Task<Result<UserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = userRepository.GetById(request.Id);
            if(user == null)
            {
                return Result<UserDto>.Error("User not found");
            }

            return Result<UserDto>.Success(mapper.Map<UserDto>(user));
        }
    }
}
