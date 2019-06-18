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

        public DbSet<WatchlistItem> WatchlistItems { get; set; }
    }
}
