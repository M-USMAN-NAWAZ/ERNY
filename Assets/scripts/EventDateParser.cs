using System;
using System.Globalization;

public static class EventDateParser
{
#if UNITY_IOS && !UNITY_EDITOR
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern int ErnyGetLocalCalendarDate();
#endif

    public static DateTime Today
    {
        get
        {
#if UNITY_IOS && !UNITY_EDITOR
            int localDate = ErnyGetLocalCalendarDate();
            return new DateTime(localDate / 10000, localDate / 100 % 100, localDate % 100);
#else
            return DateTime.Today;
#endif
        }
    }

    public static bool TryParse(string value, out DateTime date)
    {
        date = default;
        if (!DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture,
                DateTimeStyles.AllowWhiteSpaces, out DateTimeOffset timestamp))
            return false;

        // Event dates are calendar days, not instants to convert to the device's timezone.
        date = timestamp.Date;
        return true;
    }
}
