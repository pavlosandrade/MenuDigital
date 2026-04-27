using MediatR;
using MenuDigital.Domain.Shared;

namespace MenuDigital.Application.Tenants.Commands.CreateTenant;

/// <summary>
/// Representa a intenção do usuário de criar um novo Restaurante/Tenant.
/// </summary>
public sealed record CreateTenantCommand(string Name, string Slug) : IRequest<Result<Guid>>;
