using FilmQueue.WebApi.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.DataAccess
{
    public class FilmQueueDbContextUnitOfWork : IUnitOfWork
    {
        private readonly FilmQueueDbContext _dbContext;

        public FilmQueueDbContextUnitOfWork(FilmQueueDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> Execute<T>(Func<Task<T>> work)
        {
            var result = await work();

            await _dbContext.SaveChangesAsync();

            return result;
        }

        public async Task Execute(Func<Task> work)
        {
            await work();

            await _dbContext.SaveChangesAsync();
        }

        public void Execute(Action work)
        {
            work();

            _dbContext.SaveChanges();
        }
    }
}
