// Importa os novos "Instaladores" que criamos
using Hypesoft.Application;
using Hypesoft.Infrastructure.Configurations;

var builder = WebApplication.CreateBuilder(args);


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

app.Run();