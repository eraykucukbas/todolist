using System.Reflection;
using System.Text;
using System.Text.Json;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TodoApp.API.Filters;
using TodoApp.API.Middlewares;
using TodoApp.API.Modules;
using TodoApp.Core.Models;
using TodoApp.Repository.Configurations;
using TodoApp.Repository.Models;
using TodoApp.Service.Mapping;
using TodoApp.Service.Validations;
using Swashbuckle.AspNetCore.Filters;
using TodoApp.Services;
using static System.TimeSpan;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>  options.Filters.Add(new ValidateFilterAttribute())).AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<TodoListDtoValidator>());

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;

});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMemoryCache();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
    {
        Description = "Standard Authorization header using the Bearer scheme(\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});


builder.Services.AddScoped(typeof(NotFoundFilter<>));
builder.Services.AddAutoMapper(typeof(MapProfile));



builder.Services.AddIdentity<UserApp, IdentityRole>(Opt =>
{
    Opt.User.RequireUniqueEmail = true;
    Opt.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

builder.Services.Configure<CustomTokenOption>(builder.Configuration.GetSection("TokenOption"));


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
{
    var tokenOptions = builder.Configuration.GetSection("TokenOption").Get<CustomTokenOption>();
    opts.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = tokenOptions.Issuer,
        ValidAudience = tokenOptions.Audience[0],
        IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),

        ValidateIssuerSigningKey = true,
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ClockSkew = Zero
    };
});


// DB connection
builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), option =>
    {
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext))?.GetName().Name);
    });
});

// Dependency Injection
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new RepoServiceModule()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCustomException();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
