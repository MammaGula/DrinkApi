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

// Repository Layer
builder.Services.AddScoped<IDrinkRepository, DrinkRepository>();

// Service Layer
builder.Services.AddScoped<IDrinkService, DrinkService>();

// CORS: Cross Origin Resource Sharing
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

var app = builder.Build();

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
app.UseAuthorization();

// Map Controllers
app.MapControllers();

app.Run();



