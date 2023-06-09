﻿using Application.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataLayer(
            this IServiceCollection services, IConfiguration configuration)
        {
            const string connectionString = @"Host=host.docker.internal;Port=5432;Database=chippingapplicationdb;Username=admin;Password=admin";

            services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
            {
                options.EnableSensitiveDataLogging();
                options.UseNpgsql(connectionString);
            });

            return services;
        }
    }
}
