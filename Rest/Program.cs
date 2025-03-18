using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Rest.Data;
using System;
using System.Net;
using System.Threading.RateLimiting;
using WebshopLib.Services.Interfaces;
using WebshopLib.Services.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region Add ASP.NET CORE Identity
builder.Services.AddDbContext<AuthDbContext>(options =>
{
    options.UseInMemoryDatabase("AuthDb");
}
);

builder.Services.AddAuthorization();
#region Email service
builder.Services.AddTransient<IEmailSender, EmailSender>();
#endregion
builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    //Sets the Identity to include roles
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>();
#endregion

#region Manage Identity options with Ratelimiting
builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    options.Lockout.MaxFailedAccessAttempts = 3;
});
#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Manage HSTS(HTTP,Strict,Transport,Security)
builder.Services.AddHsts(options =>
{
    options.MaxAge = TimeSpan.FromSeconds(63072000);//2 years in seconds
    options.IncludeSubDomains = true;
    options.Preload = true;
});
#endregion


builder.Services.AddHttpsRedirection(options =>
{
    options.RedirectStatusCode = (int)HttpStatusCode.TemporaryRedirect;
    options.HttpsPort = 5001;
});

builder.Services.AddSingleton<IProductRepository>(new ProductRepository());
builder.Services.AddSingleton<IUserRepository>(new UserRepository());

// Register UserRepository as a singleton using a factory method and IServiceScopeFactory
builder.Services.AddSingleton(provider =>
{
    var scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();
    return new AuthManagerRepository(scopeFactory);
});

#region Commented out - Add Rate Limiter for API endpoints Manually
//builder.Services.AddRateLimiter(options =>
//{
//    options.AddPolicy("Fixed-by-user-id", context => RateLimitPartition.GetSlidingWindowLimiter(context.User.Identity?.Name, _ => new SlidingWindowRateLimiterOptions
//    {
//        Window = TimeSpan.FromMinutes(1),
//        SegmentsPerWindow = 1,
//        PermitLimit = 3
//    }));
//}
//);
#endregion

#region CORS configurations
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAny",
    builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
);
    options.AddPolicy("AllowSpecificOrigin",
    builder => builder.WithOrigins("https://localhost:9000").AllowAnyHeader().AllowAnyMethod().AllowCredentials()
    );
    options.AddPolicy("AllowOnlyGET",
   builder => builder.AllowAnyOrigin().AllowAnyHeader().WithMethods("GET")
   );
});
#endregion


#region SessionHandling
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
#endregion

#region Configure Cookiesettings for IdentityCookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Lax; 
    options.Cookie.Name = ".AspNetCore.Identity.Application";
    options.Cookie.IsEssential = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(1); 
    options.SlidingExpiration = true; 
});
#endregion



var app = builder.Build();

#region Security Middleware
app.Use(async (context, next) =>
{
    // Allowed HTTP methods
    var allowMethods = new HashSet<string> { "GET", "POST", "OPTIONS" };
    if (!allowMethods.Contains(context.Request.Method))
    {
        context.Response.StatusCode = 405;
        await context.Response.WriteAsync("Method Not Allowed");
        return;
    }

    // Allowed Content Types
    var allowedContentTypes = new HashSet<string> { "application/json", "text/plain" };
    if (context.Request.ContentType != null && !allowedContentTypes.Contains(context.Request.ContentType))
    {
        context.Response.StatusCode = StatusCodes.Status415UnsupportedMediaType;
        await context.Response.WriteAsync("Unsupported Media Type");
        return;
    }

    // Add security headers
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    //context.Response.Headers.Add(
    //    "Content-Security-Policy",
    //    "default-src 'self'; " +
    //    "script-src 'self'; " +
    //    "object-src 'none'; " +
    //    "frame-ancestors 'none'; " +
    //    "upgrade-insecure-requests; " +
    //    "base-uri 'self';"
    //);

    await next();
});
#endregion

app.UseHsts();
app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();

#region Remember to map the identity to the api here
app.MapIdentityApi<IdentityUser>();
#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

#region Make scope/script that creates roles and adds an admin at startup
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "Admin", "Manager", "User" };

    foreach (var role in roles)
    {
        if(!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    var email = "Default@Email.dk";

    var admin = new IdentityUser
    {
        UserName = email,
        Email = email
    };

    await userManager.CreateAsync(admin, "This123!it");
    var userFound = await userManager.FindByEmailAsync(email);
    userFound.EmailConfirmed = true;
    await userManager.AddToRoleAsync(userFound, "Admin");
    IUserRepository _userRepo = new UserRepository();
    _userRepo.UpdateAdminId(userFound.Id);
}
#endregion

#region Commented out - Use and Apply rate limiting to Identity endpoints currently
//app.UseRateLimiter();

//// Apply rate limiting to Identity endpoints
//app.UseEndpoints(endpoints =>
//{

//    // Apply rate limiting to specific Identity endpoints
//    endpoints.MapPost("/login", context =>
//    {
//        // Your login logic here
//        return (Task)Results.Ok("Login attempt");
//    }).RequireRateLimiting("Fixed-by-user-id");

//    endpoints.MapPost("/Identity/Account/Register", context =>
//    {
//        // Your registration logic here
//        return (Task)Results.Ok("Registration attempt");
//    }).RequireRateLimiting("Fixed-by-user-id");
//});
#endregion


app.Run();