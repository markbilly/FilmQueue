using FilmQueue.WebApi.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.DataAccess
{
    public class CacheKeys
    {
        public static string WatchNext(string userId) => userId + "_WatchNext";
    }
}
