namespace TumorHospital.Application.Intefaces.ExternalServices
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        string? UserRole { get; }
        string? Username { get; }
        string? UserEmail { get; }
    }
}
