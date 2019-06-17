using FilmQueue.WebApi.DataAccess.Models;
using FilmQueue.WebApi.Domain;
using FilmQueue.WebApi.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.DataAccess
{
    public interface ISymptomLogWriter : IDependency
    {
        Task CreateSymptomLog(long symptomId, SymptomSeverity severity);
    }

    public class SymptomLogWriter : ISymptomLogWriter
    {
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly FilmQueueDbContext _dbContext;
        private readonly IClock _clock;

        public SymptomLogWriter(
            ICurrentUserAccessor currentUserAccessor,
            FilmQueueDbContext dbContext,
            IClock clock)
        {
            _currentUserAccessor = currentUserAccessor;
            _dbContext = dbContext;
            _clock = clock;
        }

        public async Task CreateSymptomLog(long symptomId, SymptomSeverity severity)
        {
            await _dbContext.AddAsync(new SymptomLog
            {
                SymptomId = symptomId,
                Severity = severity,
                CreatedDateTime = _clock.UtcNow,
                UserId = _currentUserAccessor.CurrentUser.Id
            });
        }
    }
}
