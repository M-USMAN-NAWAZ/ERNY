using System;

public static class UserSubscriptionState
{
    public static bool IsLoaded { get; private set; }
    public static bool IsSubscribed { get; private set; }
    public static string SubscriptionType { get; private set; } = "";
    public static string SubscriptionEndDate { get; private set; } = "";

    public static string DisplayText
    {
        get { return IsSubscribed ? "Pro" : " Basic"; }
    }

    public static int PaidValue
    {
        get { return IsSubscribed ? 1 : 0; }
    }

    public static void SetLoading()
    {
        IsLoaded = false;
    }

    public static void SetFromApi(string subscriptionType, string subscriptionEndDate)
    {
        IsLoaded = true;
        SubscriptionType = subscriptionType ?? "";
        SubscriptionEndDate = subscriptionEndDate ?? "";
        IsSubscribed = HasActiveSubscription(SubscriptionType, SubscriptionEndDate);
    }

    public static void SetSubscribed(string subscriptionType, string subscriptionEndDate)
    {
        IsLoaded = true;
        SubscriptionType = subscriptionType ?? "";
        SubscriptionEndDate = subscriptionEndDate ?? "";
        IsSubscribed = true;
    }

    public static void SetUnsubscribed()
    {
        IsLoaded = true;
        SubscriptionType = "";
        SubscriptionEndDate = "";
        IsSubscribed = false;
    }

    private static bool HasActiveSubscription(string subscriptionType, string subscriptionEndDate)
    {
        if (string.IsNullOrWhiteSpace(subscriptionType))
        {
            return false;
        }

        string normalizedType = subscriptionType.Trim().ToLowerInvariant();
        if (normalizedType.Contains("no subscription") ||
            normalizedType == "basic" ||
            normalizedType == "open" ||
            normalizedType == "free")
        {
            return false;
        }

        if (DateTime.TryParse(subscriptionEndDate, out DateTime endDate))
        {
            return endDate > DateTime.Now;
        }

        return true;
    }
}
