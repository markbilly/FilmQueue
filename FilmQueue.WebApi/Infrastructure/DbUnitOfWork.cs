using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Infrastructure
{
    public interface IDbUnitOfWork<TDbContext> where TDbContext : DbContext
    {
        TDbContext DbContext { get; }

        Task<T> Execute<T>(Func<Task<T>> work);
        Task Execute(Func<Task> work);

        void Execute(Action work);
    }


    public class DbUnitOfWork<TDbContext> : IDbUnitOfWork<TDbContext> where TDbContext : DbContext
    {
        public DbUnitOfWork(TDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public TDbContext DbContext { get; }

        public async Task<T> Execute<T>(Func<Task<T>> work)
        {
            var result = await work();

            await DbContext.SaveChangesAsync();

            return result;
        }

        public async Task Execute(Func<Task> work)
        {
            await work();

            await DbContext.SaveChangesAsync();
        }

        public void Execute(Action work)
        {
            work();

            DbContext.SaveChanges();
        }
    }
}
