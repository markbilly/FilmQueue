﻿using FilmQueue.WebApi.DataAccess.Models;
using FilmQueue.WebApi.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.DataAccess
{
    public interface ISymptomReader : IDependency
    {
        Task<IEnumerable<Symptom>> Query(string searchTerm);
        Task<Symptom> GetByName(string name);
    }

    public class SymptomReader : ISymptomReader
    {
        private readonly FilmQueueDbContext _dbContext;

        public SymptomReader(FilmQueueDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Symptom> GetByName(string name)
        {
            return await _dbContext.Symptoms.FirstOrDefaultAsync(symptom => name.Equals(symptom.Name, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<IEnumerable<Symptom>> Query(string searchTerm)
        {
            return await _dbContext.Symptoms
                .Where(symptom => symptom.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
        }
    }
}