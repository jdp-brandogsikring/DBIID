using AutoMapper;
using DBIID.Application.Common.Handlers;
using DBIID.Shared.Features.Companies;
using DBIID.Shared.Features.Users;
using DBIID.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Application.Features.Users
{
    class GetUserQueryHandler : IQueryHandler<GetUserQuery, Result<UserWithAssignedCompaniesDto>>
    {
        private readonly IUserRepository userRepository;
        private readonly ILinkUserCompanyRepository linkUserCompanyRepository;
        private readonly IMapper mapper;

        public GetUserQueryHandler(IUserRepository userRepository,
                                   ILinkUserCompanyRepository linkUserCompanyRepository,
                                    IMapper mapper)
        {
            this.userRepository = userRepository;
            this.linkUserCompanyRepository = linkUserCompanyRepository;
            this.mapper = mapper;
        }
        public async Task<Result<UserWithAssignedCompaniesDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = userRepository.GetById(request.Id);
            if(user == null)
            {
                return Result<UserWithAssignedCompaniesDto>.Error("User not found");
            }

            var result = new UserWithAssignedCompaniesDto()
            {
                Id = user.Id,
                Email = user.Email,
                FamilyName = user.FamilyName,
                GivenName = user.GivenName,
                Companies = new List<CompanyDto>()
            };

            var links = linkUserCompanyRepository.GetAllIncludeCompanies().Where(x => x.UserId == user.Id);
            foreach (var link in links)
            {
                result.Companies.Add(new CompanyDto()
                {
                    Id = link.CompanyId,
                    Name = link.Company!.Name
                });
            }


            return Result<UserWithAssignedCompaniesDto>.Success(result);
        }
    }
}
