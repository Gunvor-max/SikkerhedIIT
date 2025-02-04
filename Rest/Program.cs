using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rest.Data;
using WebshopLib.Services.Interfaces;
using WebshopLib.Services.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region Adding ASP.NET CORE Identity
builder.Services.AddDbContext<AuthDbContext>(options =>
{
    options.UseInMemoryDatabase("AuthDb");
}
);

builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    //Sets the Identity to include roles
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>();
#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IProductRepository>(new ProductRepository());
builder.Services.AddSingleton<IUserRepository>(new UserRepository());

// Register UserRepository as a singleton using a factory method and IServiceScopeFactory
builder.Services.AddSingleton(provider =>
{
    var scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();
    return new AuthManagerRepository(scopeFactory);
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAny",
    builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
);
    options.AddPolicy("AllowSpecificOrigin",
    builder => builder.WithOrigins("http://zealand.dk").AllowAnyHeader().AllowAnyMethod()
    );
    options.AddPolicy("AllowOnlyGET",
   builder => builder.AllowAnyOrigin().AllowAnyHeader().WithMethods("GET")
   );
});

var app = builder.Build();

#region Remember to map the identity to the api here
app.MapIdentityApi<IdentityUser>();
#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAny");

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

//Make a scope that runs each time the application loads
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

//using (var scope = app.Services.CreateScope())
//{
//    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

//    var email = "";
//    var userFound = await userManager.FindByEmailAsync(email);
//    if (userFound != null)
//    {
//        userManager.AddToRoleAsync(userFound, "User");
//    }
//}

app.Run();
