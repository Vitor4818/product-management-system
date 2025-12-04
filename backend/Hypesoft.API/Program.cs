using Hypesoft.Application;
using Hypesoft.Infrastructure.Configurations;
<<<<<<< Updated upstream
using Hypesoft.Application.Extensions; 
using Hypesoft.API.Middlewares; 
using Serilog;
=======
using Hypesoft.Application.Extensions;
using Hypesoft.API.Middlewares;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;
using Microsoft.Extensions.Caching.Distributed;

>>>>>>> Stashed changes


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/hypesoft_log.txt", rollingInterval: RollingInterval.Day) 
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// utiliza o serilg
<<<<<<< Updated upstream
builder.Host.UseSerilog(); 
=======
builder.Host.UseSerilog();
>>>>>>> Stashed changes

// Registra o Cache In-Memory
builder.Services.AddDistributedMemoryCache();

// Todos os serviços da camada de Aplicação
builder.Services.AddApplicationServices();

// Todos os serviços da camada de Infraestrutura
builder.Services.AddInfrastructureServices(builder.Configuration);

// Serviços Padrão
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Keycloak
// app.UseAuthentication();
// app.UseAuthorization();

app.MapControllers();
app.UseDeveloperExceptionPage();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.Run();