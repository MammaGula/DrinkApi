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
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://127.0.0.1:5500", "http://localhost:5500")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
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

// HTTPS Redirection
app.UseHttpsRedirection();

// CORS
app.UseCors("AllowFrontend");

// Authentication/Authorization
app.UseAuthorization();

// Map Controllers
app.MapControllers();

app.Run();



