using MenuDigital.Api.Endpoints;
using MenuDigital.Api.Services;
using MenuDigital.Application;
using MenuDigital.Application.Interfaces;
using MenuDigital.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Explorador de Minimal APIs
builder.Services.AddEndpointsApiExplorer();

// Acessório vital para extrairmos o cabeçalho X-Tenant-ID na API
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITenantService, CurrentTenantService>();

// Tratamento de Erros Global (RFC 7807 Problem Details)
builder.Services.AddExceptionHandler<MenuDigital.Api.Infrastructure.GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// ============================================
// INJEÇÃO DA NOSSA ARQUITETURA LIMPA (CLEAN)
// ============================================
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // A integração OpenAPI mudou no .NET mais novo. Mantemos limpo por enquanto.
}

app.UseHttpsRedirection();

// Captura exceções e devolve nosso ProblemDetails padronizado
app.UseExceptionHandler();

// Registro das rotas (Minimal APIs)
app.MapTenantEndpoints();
app.MapMenuEndpoints();
app.MapCategoryEndpoints();
app.MapProductEndpoints();

app.Run();
