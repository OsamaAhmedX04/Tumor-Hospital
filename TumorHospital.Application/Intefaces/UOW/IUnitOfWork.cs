using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TumorHospital.Application.Intefaces.Repositories;

namespace TumorHospital.Application.Intefaces.UOW
{
    public interface IUnitOfWork
    {
        IRepository<T> Repo<T>() where T : class;
        Task<int> CompleteAsync();
    }
}
