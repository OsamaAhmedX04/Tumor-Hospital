namespace TumorHospital.Domain.Constants
{
    public static class SystemRole
    {
        public const string Doctor = "Doctor";
        public const string Patient = "Patient";
        public const string Admin = "Admin";
        public const string Receptionist = "Receptionist";
        public const string InActiveRole = "InActiveReceptionistRole,InActiveDoctorRole";
        public const string ActiveRole = "Doctor,Patient,Admin";
    }
}
