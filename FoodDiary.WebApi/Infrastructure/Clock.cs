using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDiary.WebApi.Infrastructure
{
    public interface IClock : IDependency
    {
        DateTime UtcNow { get; }
    }

    public class DefaultClock : IClock
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
