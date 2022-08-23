using GraphQL.Server.Ui.Voyager;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using System.Reflection;
using TestGraphQLMiddleware.Graphql;

namespace TestGraphQLMiddleware;
public class Startup
{
    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        Configuration = configuration;
        Environment = environment;
    }

    public IConfiguration Configuration { get; }
    private IWebHostEnvironment Environment { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {

        services.AddControllers();
        services.AddGraphQLSupport(Configuration);



        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "ContentBase.API", Version = "v1" });
        });

        services.AddAuthentication(Configuration);

        services.AddAuthorization(options =>
        {
            options.AddPolicy("SalesDepartment",
                policy => policy.Requirements.Add(new SalesDepartmentRequirement()));
        });

        services.AddSingleton<Microsoft.AspNetCore.Authorization.IAuthorizationHandler, SalesDepartmentAuthorizationHandler>();
        //services.AddSingleton<HotChocolate.AspNetCore.Authorization.IAuthorizationHandler, BasicAuthorizationHandler>();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder =>
                {
                    builder
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
                });
        });


    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ContentBase.API v1"));
        }

        app.UseRouting();


        app.UseAuthentication();
        app.UseAuthorization();

        app.UseCors("AllowAll");

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGraphQL("/graphql");
        });

        app.UseGraphQLVoyager(new VoyagerOptions()
        {
            GraphQLEndPoint = "/graphql",
        });
    }
}
