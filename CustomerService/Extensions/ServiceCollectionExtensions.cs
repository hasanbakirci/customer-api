using Core.Repositories.Settings;
using CustomerService.Clients.AuthClients;
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

namespace CustomerService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(MongoDB.Bson.BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(MongoDB.Bson.BsonType.String));

            services.Configure<MongoSettings>(configuration.GetSection(nameof(MongoSettings)));
            services.AddSingleton<IMongoSettings>(d=>d.GetRequiredService<IOptions<MongoSettings>>().Value);
            
            services.AddSingleton<ICustomerRepository, CustomerRepository>();
            
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<ICustomersService, CustomersService>();
            
            return services;
        }
        public static IServiceCollection AddClients(this IServiceCollection services)
        {
            services.AddSingleton<IAuthClient, AuthClient>();
            services.AddHttpClient();
            return services;
        }

        public static IServiceCollection AddUtilities(this IServiceCollection services)
        {
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