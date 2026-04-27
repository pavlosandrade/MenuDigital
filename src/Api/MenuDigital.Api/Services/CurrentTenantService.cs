using MenuDigital.Application.Interfaces;

namespace MenuDigital.Api.Services;

public sealed class CurrentTenantService : ITenantService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentTenantService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetTenantId()
    {
        // Em um cenário de produção extraímos do Header HTTP, do Claims JWT (User) ou do Subdomínio
        var context = _httpContextAccessor.HttpContext;
        
        if (context != null && context.Request.Headers.TryGetValue("X-Tenant-ID", out var tenantIdStr))
        {
            if (Guid.TryParse(tenantIdStr, out var tenantId))
            {
                return tenantId;
            }
        }

        // Se a requisição for livre de Tenant (ex: Criando a conta inicial) ou não enviar header.
        return Guid.Empty;
    }
}
