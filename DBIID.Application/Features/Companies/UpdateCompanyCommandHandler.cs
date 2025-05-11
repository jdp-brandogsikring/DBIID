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
    public class UpdateCompanyCommandHandler : ICommandHandler<UpdateCompanyCommand, Result<CompanyDto>>
    {
        private readonly ICompanyRepository companyRepository;

        public UpdateCompanyCommandHandler(ICompanyRepository companyRepository)
        {
            this.companyRepository = companyRepository;
        }

        public async Task<Result<CompanyDto>> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = companyRepository.GetById(request.Id);
            if (company == null)
            {
                return Result<CompanyDto>.Error("Company not found");
            }
            company.Name = request.Name;
            companyRepository.Update(company);
            var companyDto = new CompanyDto
            {
                Id = company.Id,
                Name = company.Name,
            };
            return Result<CompanyDto>.Success(companyDto);
        }
    }
}
