namespace GameWatch.Utilities;

public static class DateTimeHelpers
{
    public static long ConvertDateTimeToUnix(this DateTime dateTime)
    {
        var unixEpoch = (long)dateTime.Subtract(DateTime.UnixEpoch).TotalSeconds;
        return unixEpoch;
    }
}
