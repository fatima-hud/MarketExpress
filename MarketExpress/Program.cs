using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Helping;
using MarketExpress.Database;
using MarketExpress.Extensions;
using MarketExpress.Hubs;
using MarketExpress.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Serilog;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenJwtAuth();

builder.Host.UseSerilog((context,configuration)=>
configuration.ReadFrom.Configuration(context.Configuration));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("https://fashion-one-xi.vercel.app",
            "http://localhost:5173") //  „ «· ⁄œÌ· · Ã‰» „‘«ﬂ· «·‹ CORS √À‰«¡ «· Ã—»… ⁄·Ï «·”Ì—›—
              .AllowAnyHeader()
              .AllowAnyMethod()
               .AllowCredentials();
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

#if DEBUG
    options.EnableSensitiveDataLogging()
           .LogTo(Console.WriteLine, LogLevel.Information);
#endif
});

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JWT"));

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
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
        options.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
    });

builder.Services.AddCustomJwtAuth(builder.Configuration);
builder.Services.AddSignalR();

var app = builder.Build();

// 1. [ ⁄œÌ· „Â„]: ≈Ìﬁ«› «· ÕÊÌ· «· ·ﬁ«∆Ì ·‹ HTTPS ·√‰ Somee «·„Ã«‰Ì ·« Ìœ⁄„Â
/*if (!app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}*/
app.UseSerilogRequestLogging();
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Application started successfully");
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowFrontend");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var dbContext = services.GetRequiredService<ApplicationDbContext>();
    await IdentitySeeder.SeedDataAsync(roleManager, dbContext);
}

// 2. [ ⁄œÌ· „Â„]:  „ ≈Ìﬁ«› «·‹ Migration «· ·ﬁ«∆Ì · Ã‰» «‰ÂÌ«— «·”Ì—›— »”»» «·’·«ÕÌ« 
// try
// {
//     if (app.Environment.IsProduction())
//     {
//         using var scope = app.Services.CreateScope();
//         var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//         db.Database.Migrate();
//     }
// }
// catch (Exception ex)
// {
//     Console.WriteLine(ex.Message);
// }

// 3. [ ⁄œÌ· „Â„]: ≈⁄œ«œ «·”Ê«Ã— ·Ì⁄„· ›Ê—« ⁄‰œ › Õ —«»ÿ «·„Êﬁ⁄ „»«‘—…
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "MarketExpress API V1");
   
    options.EnableTryItOutByDefault();
});

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<ChatHub>("/chatHub");

app.Run();