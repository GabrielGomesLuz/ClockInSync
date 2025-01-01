using ClockInSync.Repositories.ClockInSync.Mapper;
using ClockInSync.Repositories.DbContexts;
using Microsoft.EntityFrameworkCore;
using ClockInSync.Services.Microsoft.DependencyInjection;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
 
}

builder.Services.AddDbContext<ClockInSyncDbContext>(options =>
{
    var connectionString = builder.Configuration.GetValue<string>("ConnectionString"); ;

    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddAutoMapper(typeof(EntitiesToDtoMappingProfile));
builder.Services.AddClockInSyncServices();

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true; // Assume versão padrão se não especificada
    options.DefaultApiVersion = new ApiVersion(1, 0);   // Define a versão padrão
    options.ReportApiVersions = true;                    // Permite que as versões da API sejam reportadas
});
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ClockInSyncDbContext>();
    await dbContext.Database.MigrateAsync();  // Isso aplica as migrations pendentes automaticamente
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();
