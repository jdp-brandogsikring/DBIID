using DBIID.Application.Features.Auth;
using DBIID.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DBIID.API.Service
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public int UserId
        {
            get
            {
                string? uid = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (int.TryParse(uid, out int userId))
                {
                    return userId;
                }
                else
                {
                    return 0;

                }
            }
        }
    }
}
