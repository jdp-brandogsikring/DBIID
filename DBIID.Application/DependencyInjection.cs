﻿using DBIID.Application.Features.Auth;
using DBIID.Application.Features.Users;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

            });
            return services;
        }

        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfil));
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IOtpService, OtpService>();
            services.AddScoped<IUserSyncService, UserSyncService>();

            return services;
        }


    }
}
