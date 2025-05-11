using DBIID.Application.Common.Handlers;
using DBIID.Shared.Features.Companies;
using DBIID.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Application.Features.Companies
{
    public class GetAllCompaniesQueryHandler : IQueryHandler<GetAllCompaniesQuery, Result<List<CompanyDto>>>
    {
        private readonly ICompanyRepository companyRepository;

        public GetAllCompaniesQueryHandler(ICompanyRepository companyRepository)
        {
            this.companyRepository = companyRepository;
        }

        public async Task<Result<List<CompanyDto>>> Handle(GetAllCompaniesQuery request, CancellationToken cancellationToken)
        {
            var companies = companyRepository.GetAllAsNonTracking();
            var companyDtos = companies.Select(c => new CompanyDto
            {
                Id = c.Id,
                Name = c.Name,
            }).ToList();
            return Result<List<CompanyDto>>.Success(companyDtos);
        }
    }
}
