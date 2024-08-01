using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Infrastructure.Utilities;

namespace InternSystem.Infrastructure.Services
{
    public class TimeService : ITimeService
    {
        public DateTimeOffset SystemTimeNow => DateTimeParsing.ConvertToUtcPlus7(DateTimeOffset.Now);
    }
}
