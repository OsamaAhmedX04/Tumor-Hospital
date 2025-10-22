using TumorHospital.Application.Intefaces.Repositories;
using TumorHospital.Application.Intefaces.UOW;
using TumorHospital.Domain.Entities;
using TumorHospital.Infrastructure.Persistence.Context;
using TumorHospital.Infrastructure.Persistence.Repositories;

namespace TumorHospital.Infrastructure.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;
        public IRepository<RefreshTokenAuth> RefreshTokenAuths { get; }
        

        public UnitOfWork(AppDbContext db)
        {
            _db = db;
            RefreshTokenAuths = new Repository<RefreshTokenAuth>(_db);
            
        }

        public async Task<int> CompleteAsync() => await _db.SaveChangesAsync();

        public void Dispose() => _db.Dispose();


    }
}
