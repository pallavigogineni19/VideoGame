using VideoGame.Data;
using Microsoft.EntityFrameworkCore;
using VideoGame.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure EF Core targeting local SQL Express Instance via LocalDB
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Dependency Injection
builder.Services.AddApplicationInfrastructure();


// Configure Relaxed CORS policy allowing local Angular server (port 4200) full pipeline access
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAngular", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader());
});



builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
});



var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseGlobalExceptionHandler();

// Activate cross-origin configurations
app.UseCors("AllowAngular");

app.MapControllers();

// db migration and seeding on application startup
await app.InitializeDatabaseAsync();

app.Run();