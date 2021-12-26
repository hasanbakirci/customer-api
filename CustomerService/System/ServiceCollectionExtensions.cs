using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Repositories.Settings;
using CustomerService.Repositories;
using CustomerService.Repositories.Interfaces;
using CustomerService.Services;
using CustomerService.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace CustomerService.System
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomizedService(this IServiceCollection services, IConfiguration configuration){

            services.AddSingleton<ICustomersService, CustomersService>();

            services.AddSingleton<ICustomerRepository, CustomerRepository>();


            BsonSerializer.RegisterSerializer(new StringSerializer(MongoDB.Bson.BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(MongoDB.Bson.BsonType.String));

            services.Configure<MongoSettings>(configuration.GetSection(nameof(MongoSettings)));
            services.AddSingleton<IMongoSettings>(d=>d.GetRequiredService<IOptions<MongoSettings>>().Value);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CustomerService", Version = "v1" });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowMyOrigin", builder =>
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            return services;
        }
    }
}