using Microsoft.AspNetCore.Identity;
namespace TumorHospital.Domain.Entities
{
    public class ApplicationUserToken : IdentityUserToken<string>
    {
        public DateTime ExpireDate { get; set; }
    }
}
