using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using TodoApp.Service.Mapping;
using TodoApp.Service.Validations;
using TodoApp.Web;
using TodoApp.Web.Services;
using System.Reflection;
using TodoApp.Repository.Models;
using TodoApp.Web.Middleware;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
    .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<TodoItemDtoValidator>());

builder.Services.AddAutoMapper(typeof(MapProfile));
builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"),
        option => { option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext))!.GetName().Name); });
});

builder.Services.AddHttpClient<TodoItemApiService>(opt =>
{
    opt.BaseAddress = new Uri(builder.Configuration["BaseUrl"]);
});
builder.Services.AddHttpClient<TodoListApiService>(opt =>
{
    opt.BaseAddress = new Uri(builder.Configuration["BaseUrl"]);
});

builder.Services.AddHttpClient<UserApiService>(opt =>
{
    opt.BaseAddress = new Uri(builder.Configuration["BaseUrl"]);
});

builder.Services.AddHttpClient<AuthApiService>(client => 
{
    client.BaseAddress = new Uri(builder.Configuration["BaseUrl"]);
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "CustomScheme";
    options.DefaultChallengeScheme = "CustomScheme";
})
.AddScheme<AuthenticationSchemeOptions, CustomAuthenticationHandler>("CustomScheme", null);

builder.Services.AddAuthorization();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());


var app = builder.Build();

app.UseExceptionHandler("/Home/Error");
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseMiddleware<CustomAuthorizationMiddleware>();
app.UseAuthentication(); 
app.UseAuthorization(); 

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();