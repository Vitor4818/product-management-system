using Hypesoft.Application;
using Hypesoft.Infrastructure.Configurations;
using Hypesoft.Application.Extensions; 
using Hypesoft.API.Middlewares; 
using Serilog;
using Hypesoft.Application.Extensions;
using Hypesoft.API.Middlewares;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;
using Microsoft.Extensions.Caching.Distributed;



Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/hypesoft_log.txt", rollingInterval: RollingInterval.Day) 
    .CreateLogger();


var builder = WebApplication.CreateBuilder(args);

// utiliza o serilg
builder.Host.UseSerilog();


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



// Configuração do keycloak
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["Keycloak:Authority"];
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Keycloak:Authority"],
        ValidateAudience = false, 
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});
//--

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthentication(); 
app.UseAuthorization();  

app.MapControllers();
app.UseDeveloperExceptionPage();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.Run();