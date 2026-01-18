using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Infrastructure.Persistence.Context
{
    public abstract class CustomIdentityDbContext : IdentityDbContext
        <ApplicationUser,
        IdentityRole,
        string,
        IdentityUserClaim<string>,
        IdentityUserRole<string>,
        IdentityUserLogin<string>,
        IdentityRoleClaim<string>,
        ApplicationUserToken>
    {
        protected CustomIdentityDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
