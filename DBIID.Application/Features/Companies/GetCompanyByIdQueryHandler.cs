using DBIID.Application.Common.Handlers;
using DBIID.Shared.Features.Applications;
using DBIID.Shared.Features.Companies;
using DBIID.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Application.Features.Companies
{
    public class GetCompanyByIdQueryHandler : IQueryHandler<GetCompanyByIdQuery, Result<CompanyWithApplicationsDto>>
    {
        private readonly ICompanyRepository companyRepository;
        private readonly ILinkApplicationCompanyRepository linkApplicationCompanyRepository;

        public GetCompanyByIdQueryHandler(ICompanyRepository companyRepository,
                                          ILinkApplicationCompanyRepository linkApplicationCompanyRepository)
        {
            this.companyRepository = companyRepository;
            this.linkApplicationCompanyRepository = linkApplicationCompanyRepository;
        }
        public async Task<Result<CompanyWithApplicationsDto>> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
        {
            var company = companyRepository.GetById(request.Id);
            if (company == null)
            {
                return Result<CompanyWithApplicationsDto>.Error("Company not found");
            }

            var applications = linkApplicationCompanyRepository.GetAllIncludeApplications().Where(x => x.CompanyId == company.Id);

            var companyDto = new CompanyWithApplicationsDto
            {
                Id = company.Id,
                Name = company.Name,
                Applications = applications.Select(x => new ApplicationDto
                {
                    Id = x.ApplicationId,
                    Name = x.Application!.Name,
                }).ToList()
            };
            return Result<CompanyWithApplicationsDto>.Success(companyDto);
        }
    }
}
