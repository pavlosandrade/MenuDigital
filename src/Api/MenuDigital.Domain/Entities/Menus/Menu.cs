namespace MenuDigital.Domain.Entities.Menus;

using MenuDigital.Domain.Shared;

/// <summary>
/// Representa um cardápio específico (Ex: Almoço, Jantar, Happy Hour).
/// </summary>
public sealed class Menu : EntityBase
{
    private Menu() {}

    private Menu(Guid tenantId, string name, TimeSpan startTime, TimeSpan endTime) : base()
    {
        TenantId = tenantId;
        Name = name;
        StartTime = startTime;
        EndTime = endTime;
        IsActive = true;
    }

    public Guid TenantId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public TimeSpan StartTime { get; private set; }
    public TimeSpan EndTime { get; private set; }
    public bool IsActive { get; private set; }

    public static Result<Menu> Create(Guid tenantId, string name, TimeSpan startTime, TimeSpan endTime)
    {
        if (tenantId == Guid.Empty)
            return Result.Failure<Menu>(new Error("Menu.TenantInvalid", "O ID do restaurante é inválido."));

        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Menu>(new Error("Menu.NameRequired", "O nome do cardápio é obrigatório."));

        // Validação de domínio: o cardápio não pode terminar antes de começar (no mesmo dia)
        // (Nota: Lógicas de virada de noite podem ser tratadas posteriormente se necessário)
        if (startTime >= endTime)
            return Result.Failure<Menu>(new Error("Menu.InvalidSchedule", "O horário de início não pode ser maior ou igual ao horário de término."));

        return Result.Success(new Menu(tenantId, name, startTime, endTime));
    }

    public void Activate()
    {
        IsActive = true;
        UpdateTimestamp();
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdateTimestamp();
    }

    public Result UpdateSchedule(TimeSpan newStartTime, TimeSpan newEndTime)
    {
        if (newStartTime >= newEndTime)
            return Result.Failure(new Error("Menu.InvalidSchedule", "O horário de início não pode ser maior ou igual ao horário de término."));

        StartTime = newStartTime;
        EndTime = newEndTime;
        UpdateTimestamp();

        return Result.Success();
    }
}
