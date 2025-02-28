using AutoMapper;
using DBIID.Application.Shared.Dtos;
using DBIID.Application.Common.Handlers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBIID.Shared.Features.Users;

namespace DBIID.Application.Features.Users.GetAllUsers
{
    internal class GetAllUserQueryHandler : IQueryHandler<GetAllUserQuery, IEnumerable<UserDto>>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public GetAllUserQueryHandler(IUserRepository userRepository,
                                      IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<UserDto>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var users = userRepository.GetAll();
            return mapper.Map<IEnumerable<UserDto>>(users);
        }
    }
}
