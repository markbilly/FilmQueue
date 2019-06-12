using FoodDiary.WebApi.DataAccess.Models;
using FoodDiary.WebApi.Domain;
using FoodDiary.WebApi.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDiary.WebApi.DataAccess
{
    public interface ISymptomWriter : IDependency
    {
        Task Create(string name);
    }

    public class SymptomWriter : ISymptomWriter
    {
        private readonly FoodDiaryDbContext _dbContext;

        public SymptomWriter(FoodDiaryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(string name)
        {
            await _dbContext.AddAsync(new Symptom
            {
                Name = name
            });
        }
    }
}
