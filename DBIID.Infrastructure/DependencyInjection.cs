using DBIID.Application.Common.Data;
using DBIID.Application.Features.Auth;
using DBIID.Application.Features.IdentityProviders;
using DBIID.Application.Features.Users;
using DBIID.Infrastructure.Data.Commmen;
using DBIID.Infrastructure.Data.Context;
using DBIID.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<MasterDbContext>(options =>
            {
                options.UseSqlServer(connectionString, x => { x.CommandTimeout(120);});
            });
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IOtpTransactionRepository, OtpTransactionRepository>();
            services.AddScoped<IIdentityProviderRepository, IdentityProviderRepository>();

            return services;
        }
    }
}
