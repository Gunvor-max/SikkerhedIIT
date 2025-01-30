using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rest.Data;
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
    .AddEntityFrameworkStores<AuthDbContext>();
#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IProductRepository>(new ProductRepository());

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowAny");

app.MapControllers();

app.Run();
