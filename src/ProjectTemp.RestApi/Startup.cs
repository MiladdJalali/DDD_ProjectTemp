//using Autofac;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.OpenApi.Models;
//using Npgsql;
//using ProjectTemp.Application;
//using ProjectTemp.Application.Services;
//using ProjectTemp.Infrastructure;
//using ProjectTemp.Infrastructure.Services;
//using ProjectTemp.RestApi.Services;

//namespace ProjectTemp.RestApi;

//public class Startup
//{
//    private readonly IConfiguration configuration;

//    public Startup(IConfiguration configuration)
//    {
//        this.configuration = configuration;
//    }

//    public void ConfigureServices(IServiceCollection services)
//    {
//        var connectionString = configuration.GetConnectionString("DefaultConnection");

//        services.AddHttpContextAccessor();
//        services.AddLogging(configuration);
//        services.AddAutoMapper(this.GetType().Assembly);
//        services.AddApiVersioning(options => options.ReportApiVersions = true);
//        services.AddHttpCacheHeaders();
//        services.AddControllers(options => options.ReturnHttpNotAcceptable = true)
//            .ConfigureApiBehaviorOptions(options =>
//            {
//                options.InvalidModelStateResponseFactory = context =>
//                {
//                    var validationProblemDetails = new ValidationProblemDetails(context.ModelState)
//                    {
//                        Type = "",
//                        Title = "",
//                        Status = StatusCodes.Status422UnprocessableEntity,
//                        Detail = "",
//                        Instance = context.HttpContext.Request.Path
//                    };

//                    validationProblemDetails.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);

//                    return new UnprocessableEntityObjectResult(validationProblemDetails);
//                };
//            });
//        services.AddSwaggerGen(options =>
//        {
//            options.SwaggerDoc("v4", new OpenApiInfo { Title = "ProjectTemp REST API", Version = "v1" });
//        });

//        services.AddDbContextPool<WriteDbContext>(i => { i.UseNpgsql(connectionString); });
//        services.AddDbContextPool<ReadDbContext>(i => { i.UseNpgsql(connectionString); });
//    }

//    public void ConfigureContainer(ContainerBuilder builder)
//    {
//        //var applicationAssembly = typeof(ApplicationConstants).Assembly;
//        var infrastructureAssembly = typeof(WriteDbContext).Assembly;
//        builder.RegisterAssemblyTypes(infrastructureAssembly)
//            .Where(t => t.Name.EndsWith("Repository"))
//            .AsImplementedInterfaces().InstancePerLifetimeScope();

//        //builder.RegisterMediatR(applicationAssembly, typeof(LoggingBehavior<,>), typeof(TransactionBehavior<,>));

//        builder.RegisterType<SystemEntityDetector>().As<ISystemEntityDetector>().InstancePerLifetimeScope();
//        builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
//        builder.RegisterType<SystemDateTime>().As<ISystemDateTime>().InstancePerLifetimeScope();
//        builder.RegisterType<UserDescriptor>().As<IUserDescriptor>().InstancePerLifetimeScope();
//    }

//    public void Configure(
//        WriteDbContext dbContext,
//        IApplicationBuilder app,
//        IWebHostEnvironment env,
//        IServiceScopeFactory serviceScopeFactory)
//    {
//        if (env.IsDevelopment())
//        {
//            app.UseDeveloperExceptionPage();
//            app.UseSwagger();
//            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProjectTemp REST API v1"));
//        }

//        dbContext.Database.Migrate();

//        var dbConnection = dbContext.Database.GetDbConnection();
//        dbConnection.Open();
//        ((NpgsqlConnection)dbConnection).ReloadTypes();
//        dbConnection.Close();

//      //  DatabaseSeeder.Seed(serviceScopeFactory, configuration).GetAwaiter().GetResult();

//        app.UseHttpCacheHeaders();
//        app.UseRouting();
//        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
//    }
//}