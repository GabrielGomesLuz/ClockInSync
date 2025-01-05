using ClockInSync.Repositories.ClockInSync.Mapper;
using ClockInSync.Repositories.DbContexts;
using ClockInSync.Services.Microsoft.DependencyInjection;
using ClockInSync.Services.TokenServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<TokenService>();

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


builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => 
options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
{
    ValidateIssuer = true,
    ValidIssuer = builder.Configuration["Jwt:Issuer"],
    ValidateAudience = true,
    ValidAudience = builder.Configuration["Jwt:Audience"], // Deve corresponder à audiência do token
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!))
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

app.UseAuthentication();
app.UseAuthorization();



app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();
