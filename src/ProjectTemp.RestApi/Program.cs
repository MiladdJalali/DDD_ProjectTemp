using Autofac.Core;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ProjectTemp.Application;
using ProjectTemp.Application.Aggregates.Users;
using ProjectTemp.Application.Aggregates.Users.Queries.GetUserCollection;
using ProjectTemp.Application.Services;
using ProjectTemp.Infrastructure;
using ProjectTemp.Infrastructure.Aggregates.Users;
using ProjectTemp.Infrastructure.Services;
using ProjectTemp.RestApi;
using ProjectTemp.RestApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddLogging(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers(options => options.ReturnHttpNotAcceptable = true)
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var validationProblemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Type = "",
                Title = "",
                Status = StatusCodes.Status422UnprocessableEntity,
                Detail = "",
                Instance = context.HttpContext.Request.Path
            };

            validationProblemDetails.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);

            return new UnprocessableEntityObjectResult(validationProblemDetails);
        };
    }); ;
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "ProjectTemp REST API", Version = "v1" });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<WriteDbContext>(i => { i.UseNpgsql(connectionString); });
builder.Services.AddDbContext<ReadDbContext>(i => { i.UseNpgsql(connectionString); });

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserDescriptor, UserDescriptor>();
builder.Services.AddScoped<ISystemDateTime, SystemDateTime>(); ;
builder.Services.AddScoped<ISystemEntityDetector, SystemEntityDetector>();

builder.Services.AddScoped<IUserReadRepository, UserReadRepository>();
builder.Services.AddTransient<IUserWriteRepository, UserWriteRepository>();

builder.Services.AddApiVersioning(options => options.ReportApiVersions = true);
builder.Services.AddMediator(options => options.ServiceLifetime = ServiceLifetime.Scoped);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProjectTemp REST API v1"));
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<WriteDbContext>();
    dbContext.Database.Migrate();
    
   DatabaseSeeder.Seed(scope.ServiceProvider, builder.Configuration).GetAwaiter().GetResult();
}
app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();