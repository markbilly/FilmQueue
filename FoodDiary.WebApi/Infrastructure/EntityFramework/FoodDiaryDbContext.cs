using FoodDiary.WebApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodDiary.WebApi.Infrastructure.EntityFramework
{
    public class FoodDiaryDbContext : DbContext
    {
        public FoodDiaryDbContext(DbContextOptions options)
            : base(options)
        {
        }

        //public DbSet<Meal> Meals { get; set; }
    }
}
