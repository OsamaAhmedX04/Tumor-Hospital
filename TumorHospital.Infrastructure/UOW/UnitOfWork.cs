using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TumorHospital.Application.Intefaces.Repositories;
using TumorHospital.Application.Intefaces.UOW;
using TumorHospital.Infrastructure.Persistence.Context;
using TumorHospital.Infrastructure.Persistence.Repositories;

namespace TumorHospital.Infrastructure.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;

        public UnitOfWork(AppDbContext db)
        {
            _db = db;
        }

        public IRepository<T> Repo<T>() where T : class
        {
            return new Repository<T>(_db);
        }

        public async Task<int> CompleteAsync()
        {
            return await _db.SaveChangesAsync();
        }

        public void Dispose() => _db.Dispose();

       
    }
}
