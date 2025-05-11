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
    public class GetCompanyByIdQueryHandler : IQueryHandler<GetCompanyByIdQuery, Result<CompanyDto>>
    {
        private readonly ICompanyRepository companyRepository;

        public GetCompanyByIdQueryHandler(ICompanyRepository companyRepository)
        {
            this.companyRepository = companyRepository;
        }
        public async Task<Result<CompanyDto>> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
        {
            var company = companyRepository.GetById(request.Id);
            if (company == null)
            {
                return Result<CompanyDto>.Error("Company not found");
            }
            var companyDto = new CompanyDto
            {
                Id = company.Id,
                Name = company.Name,
            };
            return Result<CompanyDto>.Success(companyDto);
        }
    }
}
