namespace InternSystem.Infrastructure.Utilities
{
    public static class DateTimeParsing
    {
        public static DateTimeOffset ConvertToUtcPlus7(DateTimeOffset dateTime)
        {
            return dateTime.ToOffset(TimeSpan.FromHours(7));
        }
    }
}
