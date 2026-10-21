using System;
using System.Globalization;

public static class EventDateParser
{
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
