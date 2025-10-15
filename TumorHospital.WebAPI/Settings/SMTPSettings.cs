namespace TumorHospital.WebAPI.Settings
{
    public class SMTPSettings
    {
        public string Host { get; set; } = null!;
        public int Port { get; set; }
        public bool EnableSSL { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
    }
}
