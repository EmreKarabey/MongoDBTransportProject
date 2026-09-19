using Application.Services.Repository;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Services;
using System;

namespace Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersisitenceServiceRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ISliderRepository, SliderRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IOfferRepository, OfferRepository>();
            services.AddScoped<IAboutRepository, AboutRepository>();
            services.AddScoped<IGetInTouchRepository, GetInTouchRepository>();
            services.AddScoped<IHowItWorkRepository, HowItWorkRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IWhatWeHaveDoneRepository, WhatWeHaveDoneRepository>();
            services.AddScoped<IFAQRepository, FAQRepository>();

            var connectionString = configuration.GetSection("DatabaseSettings")["ConnectionString"];
            var databaseName = configuration.GetSection("DatabaseSettings")["DatabaseName"];

            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
                options.User.RequireUniqueEmail = true;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = true;
            })
            .AddMongoDbStores<AppUser, AppRole, Guid>(
                connectionString,
                databaseName
            )
            .AddDefaultTokenProviders();

            return services;
        }
    }
}
