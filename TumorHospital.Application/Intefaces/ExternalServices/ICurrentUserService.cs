namespace TumorHospital.Application.Intefaces.ExternalServices
{
    public interface ICurrentUserService
    {
        public string? UserId { get; }
        public string? UserRole { get; }
        public string? Username { get; }
        public string? UserEmail { get; }
    }
}
