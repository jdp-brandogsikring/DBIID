using DBIID.Application.Common.Handlers;
using DBIID.Application.Features.Auth;
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
        private readonly IUserRepository userRepository;
        private readonly ICurrentUser currentUser;
        CacheStorage cacheStorage = CacheStorage.Instance;


        public GenerateApplicationLoginCommandHandler(IApplicationRepository applicationRepository,
                                              IUserRepository userRepository,
                                              ICurrentUser currentUser)
        {
            this.applicationRepository = applicationRepository;
            this.userRepository = userRepository;
            this.currentUser = currentUser;
        }

        public async Task<Result<ApplicationLoginDto>> Handle(GenerateApplicationLoginCommand request, CancellationToken cancellationToken)
        {
            ApplicationLoginDto loginDto = new ApplicationLoginDto();

            var application = applicationRepository.GetAllAsNonTracking().FirstOrDefault(x => x.Token.ToLower() == request.Token.ToLower());
            if (application == null)
            {
                return Result<ApplicationLoginDto>.Error("Application not found");
            }

            loginDto.ApplicationId = application.Id;
            loginDto.ApplicationName = application.Name;
            loginDto.ApplicationUrl = application.Url;

            var user = userRepository.GetAllAsNonTracking().FirstOrDefault(x => x.Id == currentUser.UserId);
            if (user == null)
            {
                return Result<ApplicationLoginDto>.Error("User not found");
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
