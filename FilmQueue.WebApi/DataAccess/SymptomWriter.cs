using FilmQueue.WebApi.DataAccess.Models;
using FilmQueue.WebApi.Domain;
using FilmQueue.WebApi.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.DataAccess
{
    public interface ISymptomWriter : IDependency
    {
        Task Create(string name);
    }

    public class SymptomWriter : ISymptomWriter
    {
        private readonly FilmQueueDbContext _dbContext;

        public SymptomWriter(FilmQueueDbContext dbContext)
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
