using FilmQueue.WebApi.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FilmQueue.WebApi.DataAccess
{
    public class FilmQueueDbContext : DbContext
    {
        public FilmQueueDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Food> Foods { get; set; }
        public DbSet<FoodLog> FoodLogs { get; set; }
        public DbSet<Symptom> Symptoms { get; set; }
        public DbSet<SymptomLog> SymptomLogs { get; set; }
    }
}
