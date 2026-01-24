using DrinkApi.Data;
using DrinkApi.Middleware;
using DrinkApi.Services;
using DrinkApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();

//// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
//builder.Services.AddSwaggerGen();


// Swagger (klassisk)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Databaser DI scoped som standard, den lever vid varje request
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Service Lager
// when controller requests IProductService, let DI Container create ProductService
builder.Services.AddScoped<DrinkDbContextService>(); // tell Dependency Injection (DI) container create DrinkDbContextService
builder.Services.AddScoped<IDrinkService, DrinkService>();

//CORS: cross origin resource sharing
//Backenden blockerar anrop från andra domäner
builder.Services.AddCors(options =>
{
    options.AddPolicy("FontenPolicy", policy =>
    {
        policy.WithOrigins("http://127.0.0.1:5500")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAll", policy =>
//    {
//        policy.AllowAnyOrigin()
//              .AllowAnyMethod()
//              .AllowAnyHeader();
//    });
//});



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseCors("AllowAll");



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    /*app.MapOpenApi();*/ //create and expose OpenAPI documents automatically(openAI-json), not using Swagger
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseCors("AllowAll");
//app.UseCors("FontenPolicy");





app.UseMiddleware<ErrorHandlingMiddleware>();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();  //tell the system to use rouing from Controllers

app.Run();


