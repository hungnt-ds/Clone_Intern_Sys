namespace InternSystem.Application.Common.Services.Interfaces
{
    public interface ITimeService
    {
        DateTimeOffset SystemTimeNow { get; }
    }
}
