using AutoMapper;
using DBIID.Shared.Features.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Application
{
    internal class AutoMapperProfil : Profile
    {
        public AutoMapperProfil()
        {
            CreateMap<Domain.Entities.User, UserDto>();
        }
    }
}
