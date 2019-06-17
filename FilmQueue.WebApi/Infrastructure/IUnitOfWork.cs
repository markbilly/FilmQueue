using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Infrastructure
{
    public interface IUnitOfWork : IDependency
    {
        Task<T> Execute<T>(Func<Task<T>> work);
        Task Execute(Func<Task> work);
    }
}
