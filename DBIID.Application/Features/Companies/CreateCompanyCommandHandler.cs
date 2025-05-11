using AutoMapper;
using DBIID.Application.Common.Data;
using DBIID.Application.Common.Handlers;
using DBIID.Domain.Entities;
using DBIID.Shared.Features.Companies;
using DBIID.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Application.Features.Companies
{
    public class CreateCompanyCommandHandler : ICommandHandler<CreateCompanyCommand, Result<CompanyDto>>
    {
        private readonly ICompanyRepository companyRepository;
        private readonly IUnitOfWork unitOfWork;

        public CreateCompanyCommandHandler(ICompanyRepository companyRepository,
                                           IUnitOfWork unitOfWork)
        {
            this.companyRepository = companyRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<CompanyDto>> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = new Company
            {
                Name = request.Name,
            };
            await companyRepository.AddAsync(company);
            await unitOfWork.SaveChangesAsync();
            return Result<CompanyDto>.Success(new CompanyDto
            {
                Id = company.Id,
                Name = company.Name,
            });
        }
    }
}
