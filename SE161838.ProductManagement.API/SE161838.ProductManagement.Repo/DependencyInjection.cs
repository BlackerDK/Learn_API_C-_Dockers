using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SE161838.ProductManagement.Repo.Interface;
using SE161838.ProductManagement.Repo.Mappers;
using SE161838.ProductManagement.Repo.Models;
using SE161838.ProductManagement.Repo.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE161838.ProductManagement.Repo
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfractstructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddAutoMapper(typeof(MapperConfigs).Assembly);
            services.AddDbContext<FstoreDbContext>(opt => opt.UseSqlServer(config.GetConnectionString("DatabaseConnection")));
            return services;            
        }
    }
}
