using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Security.Authentication;
using System.Text;
using TestGraphQLMiddleware.Graphql;

namespace TestGraphQLMiddleware
{
    public static class StartupExtensions
    {

        public static IServiceCollection AddGraphQLSupport(this IServiceCollection services, IConfiguration configuration)
        {
            var graphqlService = services
                .AddGraphQLServer()
                .AddAuthorization()
                .AddQueryType()
                    .AddTypeExtension<SampleQueries>()
                .InitializeOnStartup();
            return services;
        }

       public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "https://adam.ai/auth",
                        CryptoProviderFactory = new CryptoProviderFactory()
                        {
                            CacheSignatureProviders = false
                        },
                    };
            });
            return services;
        }

    }
}
