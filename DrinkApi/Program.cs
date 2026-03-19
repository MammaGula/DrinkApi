using DrinkApi.Data;
using DrinkApi.Middleware;
using DrinkApi.Services;
using DrinkApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database - Scoped by default
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<Microsoft.AspNetCore.Identity.IdentityUser, Microsoft.AspNetCore.Identity.IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
    .AddEntityFrameworkStores<AppDbContext>();

// Repository Layer
builder.Services.AddScoped<IDrinkRepository, DrinkRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Service Layer
builder.Services.AddScoped<IDrinkService, DrinkService>();
builder.Services.AddScoped<IOrderService, OrderService>();

// CORS: Cross Origin Resource Sharing
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

var app = builder.Build();

// Seed default roles and users (run before app starts)
IdentitySeeder.SeedAsync(app.Services, builder.Configuration)
    .GetAwaiter()
    .GetResult();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Error handling
app.UseMiddleware<ErrorHandlingMiddleware>();

// HTTPS Redirection - Disabled for development
// app.UseHttpsRedirection();

// CORS
app.UseCors("AllowAll");

// Authentication/Authorization
app.UseAuthentication();
app.UseAuthorization();

// Map Controllers
app.MapControllers();

app.Run();




