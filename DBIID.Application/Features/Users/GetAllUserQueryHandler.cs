using AutoMapper;
using DBIID.Application.Common.Handlers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBIID.Shared.Features.Users;
using DBIID.Shared.Results;

namespace DBIID.Application.Features.Users
{
    internal class GetAllUserQueryHandler : IQueryHandler<GetAllUserQuery, Result<IEnumerable<UserDto>>>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public GetAllUserQueryHandler(IUserRepository userRepository,
                                      IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }
        public async Task<Result<IEnumerable<UserDto>>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var users = userRepository.GetAll();
            return Result<IEnumerable<UserDto>>.Success(mapper.Map<IEnumerable<UserDto>>(users));
        }
    }
}
