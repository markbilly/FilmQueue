using Autofac;
using FilmQueue.WebApi.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.DataAccess
{
    public class FilmQueueDbUnitOfWork : DbUnitOfWork<FilmQueueDbContext>
    {
        public FilmQueueDbUnitOfWork(FilmQueueDbContext dbContext) : base(dbContext)
        {
        }
    }

    public class FilmQueueDbUnitOfWorkModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FilmQueueDbUnitOfWork>();
        }
    }
}
