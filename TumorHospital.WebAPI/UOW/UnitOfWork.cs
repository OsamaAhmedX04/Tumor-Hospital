using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics.Metrics;
using System.Net;
using TumorHospital.WebAPI.Data;
using TumorHospital.WebAPI.Data.Models;
using TumorHospital.WebAPI.Repositories.Implementations;
using TumorHospital.WebAPI.Repositories.Interfaces;

namespace TumorHospital.WebAPI.UOW
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

        public IRepository<T> Repository<T>() where T : class
        {
            return new Repository<T>(_db);
        }
    }
}
