using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServiceRegistration(this IServiceCollection services)
        {
            services.AddAutoMapper(n => n.AddMaps(Assembly.GetExecutingAssembly()));

            services.AddMediatR(n => n.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            
            services.AddScoped<Application.Features.FAQ.Rules.FAQBusinessRules>();
            services.AddScoped<Application.Features.WhatWeHaveDone.Rules.WhatWeHaveDoneBusinessRules>();

            return services;
        }
    }
}
