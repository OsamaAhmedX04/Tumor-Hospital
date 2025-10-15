using TumorHospital.WebAPI.Data.Models;
using TumorHospital.WebAPI.Repositories.Interfaces;

namespace TumorHospital.WebAPI.UOW
{
    public interface IUnitOfWork
    {
        IRepository<RefreshTokenAuth> RefreshTokenAuths { get; }


        Task<int> CompleteAsync();
    }
}
