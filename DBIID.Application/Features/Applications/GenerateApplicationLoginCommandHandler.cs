using DBIID.Application.Common.Handlers;
using DBIID.Application.Features.Auth;
using DBIID.Application.Features.Companies;
using DBIID.Application.Features.Users;
using DBIID.Shared.Features.Applications;
using DBIID.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Application.Features.Applications
{
    public class GenerateApplicationLoginCommandHandler : IQueryHandler<GenerateApplicationLoginCommand, Result<ApplicationLoginDto>>
    {
        private readonly IApplicationRepository applicationRepository;
        private readonly ICompanyRepository companyRepository;
        private readonly IUserRepository userRepository;
        private readonly ICurrentUser currentUser;
        CacheStorage cacheStorage = CacheStorage.Instance;


        public GenerateApplicationLoginCommandHandler(IApplicationRepository applicationRepository,
                                              ICompanyRepository companyRepository,
                                              IUserRepository userRepository,
                                              ICurrentUser currentUser)
        {
            this.applicationRepository = applicationRepository;
            this.companyRepository = companyRepository;
            this.userRepository = userRepository;
            this.currentUser = currentUser;
        }

        public async Task<Result<ApplicationLoginDto>> Handle(GenerateApplicationLoginCommand request, CancellationToken cancellationToken)
        {
            ApplicationLoginDto loginDto = new ApplicationLoginDto();

            var application = applicationRepository.GetAllIncludeCompanies().FirstOrDefault(x => x.Token.ToLower() == request.Token.ToLower());
            if (application == null)
            {
                return Result<ApplicationLoginDto>.Error("Application not found");
            } 

            loginDto.ApplicationId = application.Id;
            loginDto.ApplicationName = application.Name;
            loginDto.ApplicationUrl = application.Url;

            var user = userRepository.GetAllIncludeCompanies().FirstOrDefault(x => x.Id == currentUser.UserId);
            if (user == null)
            {
                return Result<ApplicationLoginDto>.Error("User not found");
            }

            if(!application.Links.Any() || !user.Links.Any()) 
            { 
                return Result<ApplicationLoginDto>.Error("User not found in this application");
            }

            var companyId = application.Links.Where(x => user.Links.Any(y => y.CompanyId == x.CompanyId)).ToList();
            if (companyId == null || !companyId.Any())
            {
                return Result<ApplicationLoginDto>.Error("User not found in this application");
            }

            var company = companyRepository.GetById(companyId.FirstOrDefault().CompanyId);
            if (company == null)
            {
                return Result<ApplicationLoginDto>.Error("Company not found");
            }

            loginDto.UserId = user.Id;
            loginDto.UserName = user.GivenName + " " + user.FamilyName;

            loginDto.SSOToken = Guid.NewGuid().ToString().Replace("-", "");
            loginDto.Expire = DateTime.UtcNow.AddMinutes(5);

            cacheStorage.Set(loginDto.SSOToken, loginDto);

            return Result<ApplicationLoginDto>.Success(loginDto);
        }
    }
}
