using MarketExpress.Application.Abstacts;
using MarketExpress.Application.Abstracts;
using MarketExpress.Application.Implementations;
using MarketExpress.Core.Helping;
using MarketExpress.Database;
using MarketExpress.Extensions;
using MarketExpress.Infrastructure.Abstacts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MarketExpress.Infrastructure.Identity;
using MarketExpress.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using MarketExpress.Core.Entity.Identity;
using System.Text;
using MarketExpress.Infrastructure.Abstracts;
using Microsoft.EntityFrameworkCore;
using MarketExpress.Hubs;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters
            .Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenJwtAuth();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
            "https://localhost:7155",
            "http://localhost:3000",
            "http://localhost:5173"
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials(); // „Â„ ··‹ SignalR
    });
});
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")).EnableSensitiveDataLogging()
             .LogTo(Console.WriteLine, LogLevel.Information));
   ;
builder.Services.AddIdentity<User,IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JWT"));
;
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddScoped<IUrlHelper>(factory =>
{
    var actionContext = factory.GetRequiredService<IActionContextAccessor>().ActionContext;
    return new UrlHelper(actionContext);
});
builder.Services.AddApplicationServices();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
    });
builder.Services.AddCustomJwtAuth(builder.Configuration);
builder.Services.AddSignalR();
var app = builder.Build();
app.UseHttpsRedirection();
app.UseDefaultFiles();  // ÌÃ⁄· index.html «› —«÷Ì…
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowFrontend");
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await IdentitySeeder.SeedRolesAsync(roleManager);
}
// Configure the HTTP request pipeline.
// !Environment.GetCommandLineArgs().Any(arg => arg.Contains("ef"))
if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
}
app.UseSwagger();
app.UseSwaggerUI(
    options =>
    {
        options.EnableTryItOutByDefault();
    });
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<ChatHub>("/chatHub");
// ? Â–« Ì”„Õ ·Œœ„… «·„·›«  œ«Œ· wwwroot
app.Run();
