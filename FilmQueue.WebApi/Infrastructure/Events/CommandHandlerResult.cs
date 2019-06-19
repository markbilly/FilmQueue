using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Infrastructure.Events
{
    public class CommandHandlerResult
    {
        public CommandHandlerResult()
        {
            FailureMessages = new Dictionary<string, string>();
        }

        public bool WasSuccessful { get; set; }
        public Dictionary<string, string> FailureMessages { get; set; }
    }
}
