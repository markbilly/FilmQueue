using FoodDiary.WebApi.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDiary.WebApi.DataAccess
{
    public class FoodDiaryDbContextUnitOfWork : IUnitOfWork
    {
        private readonly FoodDiaryDbContext _dbContext;

        public FoodDiaryDbContextUnitOfWork(FoodDiaryDbContext dbContext)
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
    }
}
