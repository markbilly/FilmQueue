using FoodDiary.WebApi.DataAccess.Models;
using FoodDiary.WebApi.Domain;
using FoodDiary.WebApi.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDiary.WebApi.DataAccess
{
    public interface ISymptomLogWriter : IDependency
    {
        Task CreateSymptomLog(long symptomId, SymptomSeverity severity);
    }

    public class SymptomLogWriter : ISymptomLogWriter
    {
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly FoodDiaryDbContext _dbContext;
        private readonly IClock _clock;

        public SymptomLogWriter(
            ICurrentUserAccessor currentUserAccessor,
            FoodDiaryDbContext dbContext,
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
