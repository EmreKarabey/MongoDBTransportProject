using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Helsinki;
using Application.Services.Repository;
using Application.Services.ToxicBert;
using Infrastructure.ConnectionService;
using Infrastructure.Helsinki;
using Infrastructure.ToxicBert;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServiceRegistration(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddScoped<IDataBaseSettings, DataBaseSettings>();
            services.AddScoped<IHelsinkiService, HelsinkiService>();
            services.AddScoped<IToxicBertService, ToxicBertService>();

            services.Configure<DataBaseSettings>(configuration.GetSection("DatabaseSettings"));

            services.AddScoped<IDataBaseSettings>(sp =>
            {
                return sp.GetRequiredService<IOptions<DataBaseSettings>>().Value;
            });

            return services;
        }
    }
}
