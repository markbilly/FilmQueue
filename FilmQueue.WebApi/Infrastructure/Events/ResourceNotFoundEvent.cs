using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Infrastructure.Events
{
    public class ResourceNotFoundEvent : IEvent
    {
        public ResourceNotFoundEvent(long resourceId)
        {
            ResourceId = resourceId;
        }

        public long ResourceId { get; }
    }
}
