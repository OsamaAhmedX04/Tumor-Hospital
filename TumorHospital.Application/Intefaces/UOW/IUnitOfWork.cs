using TumorHospital.Application.Intefaces.Repositories;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Application.Intefaces.UOW
{
    public interface IUnitOfWork
    {
        IRepository<RefreshTokenAuth> RefreshTokenAuths { get; }
        Task<int> CompleteAsync();
    }
}
