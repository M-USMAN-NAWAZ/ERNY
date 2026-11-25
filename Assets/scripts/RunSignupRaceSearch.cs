using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using Newtonsoft.Json.Linq;
using UnityEngine;

public static class RunSignupRaceSearch
{
    public const int PageSize = 15;
    private static readonly CultureInfo US = CultureInfo.GetCultureInfo("en-US");

    // Public discovery endpoint; never embed the account API password in a player build.
    public static List<string> BuildUrls(RaceDiscoveryController.SearchFilters f, int page, DateTime today)
    {
        var common = new Dictionary<string, string> {
            { "format", "json" }, { "events", "T" }, { "sort", "date ASC" },
            { "results_per_page", PageSize.ToString() }, { "page", page.ToString() },
            { "country", f.country }, { "state", f.state }, { "zipcode", f.zip }, { "radius", f.radiusMiles },
            { "distance_units", "K" }, { "search_start_date_only", "T" }
        };
        var start = today.Date;
        if (int.TryParse(f.fromMonth, out int month))
        {
            start = new DateTime(today.Year, month, 1);
            if (start.AddMonths(1) <= today.Date) start = start.AddYears(1);
        }
        common["start_date"] = start.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        if (int.TryParse(f.toMonth, out int endMonth))
        {
            var end = new DateTime(start.Year, endMonth, 1).AddMonths(1).AddDays(-1);
            if (end < start) end = end.AddYears(1);
            common["end_date"] = end.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
        double minimum = 0, maximum = double.MaxValue;
        double factor = f.unit == "mi" ? 1.609344 : 1;
        if (double.TryParse(f.minDistance, NumberStyles.Float, CultureInfo.InvariantCulture, out double min)) minimum = min * factor;
        if (double.TryParse(f.maxDistance, NumberStyles.Float, CultureInfo.InvariantCulture, out double max)) maximum = max * factor;
        double typeDistance = 0;
        switch (f.raceType)
        {
            case "5K": typeDistance = 5; break;
            case "10K": typeDistance = 10; break;
            case "15K": typeDistance = 15; break;
            case "Half Marathon": typeDistance = 21.0975; break;
            case "Marathon": typeDistance = 42.195; break;
            case "Trail": common["event_type"] = "trail_race"; break;
            case "Obstacle": common["event_type"] = "obstacle_course"; break;
            case "Other": common["event_type"] = "other"; break;
        }
        if (typeDistance > 0)
        { minimum = Math.Max(minimum, typeDistance * .99); maximum = Math.Min(maximum, typeDistance * 1.01); }
        if (minimum > maximum) throw new ArgumentException("The selected race type is outside your distance range.");
        if (minimum > 0) common["min_distance"] = minimum.ToString("0.#####", CultureInfo.InvariantCulture);
        if (maximum != double.MaxValue) common["max_distance"] = maximum.ToString("0.#####", CultureInfo.InvariantCulture);
        if (f.raceType == "Fun Run") common["name"] = "Fun Run";
        var queries = new List<Dictionary<string,string>>();
        if (string.IsNullOrWhiteSpace(f.name)) queries.Add(common);
        else
        {
            var name = new Dictionary<string,string>(common); name["name"] = f.name; queries.Add(name);
            var city = new Dictionary<string,string>(common); city["city"] = f.name; queries.Add(city);
            string eventType = EventType(f.name);
            if (eventType != null) { var type = new Dictionary<string,string>(common); type["event_type"] = eventType; queries.Add(type); }
            if (TryDistance(f.name, out double distance))
            {
                var type = new Dictionary<string,string>(common);
                type["min_distance"] = Math.Max(minimum, distance * .99).ToString("0.#####", CultureInfo.InvariantCulture);
                type["max_distance"] = Math.Min(maximum, distance * 1.01).ToString("0.#####", CultureInfo.InvariantCulture);
                queries.Add(type);
            }
        }
        return queries.Select(q => "https://api.runsignup.com/rest/races?" + string.Join("&", q
            .Where(pair => !string.IsNullOrWhiteSpace(pair.Value))
            .Select(pair => Uri.EscapeDataString(pair.Key) + "=" + Uri.EscapeDataString(pair.Value)))).ToList();
    }

    private static string EventType(string text)
    {
        switch (text.Trim().ToLowerInvariant()) {
            case "trail": return "trail_race";
            case "virtual": return "virtual_race";
            case "obstacle": return "obstacle_course";
            case "triathlon": return "triathlon";
            case "walking": return "walking_only";
            case "running": return "running_race";
            default: return null;
        }
    }
    private static bool TryDistance(string text, out double km)
    {
        switch (text.Trim().ToLowerInvariant()) {
            case "5k": km = 5; return true;
            case "10k": km = 10; return true;
            case "15k": km = 15; return true;
            case "half marathon": km = 21.0975; return true;
            case "marathon": km = 42.195; return true;
            default: km = 0; return false;
        }
    }

    public static List<RaceDiscoveryController.RaceResult> Parse(string json)
    {
        using var reader = new Newtonsoft.Json.JsonTextReader(new System.IO.StringReader(json));
        reader.DateParseHandling = Newtonsoft.Json.DateParseHandling.None;
        var root = JObject.Load(reader);
        if (root["error"] != null) throw new FormatException("RunSignup could not complete this search.");
        var list = root["races"] as JArray;
        if (list == null) throw new FormatException("RunSignup returned an unexpected response.");
        var results = new List<RaceDiscoveryController.RaceResult>();
        foreach (var item in list)
        {
            var r = item["race"];
            if (r == null || string.IsNullOrWhiteSpace((string)r["name"])) continue;
            var events = (r["events"] as JArray) ?? new JArray();
            string dateText = (string)r["next_date"];
            if (string.IsNullOrWhiteSpace(dateText)) dateText = (string)events.FirstOrDefault()?["start_time"];
            EventDateParser.TryParse(dateText, out DateTime date);
            string endText = (string)r["next_end_date"];
            if (string.IsNullOrWhiteSpace(endText)) endText = (string)events.FirstOrDefault()?["end_time"];
            EventDateParser.TryParse(endText, out DateTime endDate);
            var address = r["address"];
            var distances = events.Select(e => (string)e["distance"]).Where(v => !string.IsNullOrWhiteSpace(v)).Distinct();
            results.Add(new RaceDiscoveryController.RaceResult {
                id = (string)r["race_id"], name = WebUtility.HtmlDecode((string)r["name"]),
                date = date == default ? "Date to be confirmed" : date.ToString("ddd M/d/yy", US)
                    + (endDate.Date > date.Date ? " - " + endDate.ToString("M/d/yy", US) : ""),
                sortDate = date, location = string.Join(", ", new[] { (string)address?["city"], (string)address?["state"] }.Where(v => !string.IsNullOrWhiteSpace(v))),
                distances = string.Join("   ", distances), logoUrl = (string)r["logo_url"], url = (string)r["url"],
                entries = events.Select(e => {
                    var eventName = WebUtility.HtmlDecode((string)e["name"]);
                    var distance = (string)e["distance"];
                    var startTime = (string)e["start_time"];
                    return new RaceDiscoveryController.RaceEntry {
                        name = string.IsNullOrWhiteSpace(eventName) ? distance : eventName,
                        date = EventDateParser.TryParse(startTime, out DateTime day) ? day : date.Date,
                        distance = distance, startTime = startTime
                    };
                }).Where(e => !string.IsNullOrWhiteSpace(e.name)).ToArray()
            });
        }
        return results;
    }

    public static eventgetter.Distance ImportDistance(string value, out string type)
    {
        var match = System.Text.RegularExpressions.Regex.Match(value ?? "",
            @"^\s*(\d+(?:\.\d+)?)\s*(k(?:m|ilometers?)?|mi(?:les?)?)\s*$",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        type = "OTHER";
        if (!match.Success || !decimal.TryParse(match.Groups[1].Value, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out decimal amount) || amount <= 0 || Math.Round(amount, 4) >= 100)
            return new eventgetter.Distance { istance = "00.00.00", unit = "km" };
        bool miles = match.Groups[2].Value.StartsWith("mi", StringComparison.OrdinalIgnoreCase);
        decimal km = miles ? amount * 1.609344m : amount;
        if (Math.Abs(km - 5) < .1m) type = "5K";
        else if (Math.Abs(km - 10) < .1m) type = "10K";
        else if (Math.Abs(km - 15) < .1m) type = "15K";
        else if (Math.Abs(km - 21.1m) < .1m) type = "HALFATHON";
        else if (Math.Abs(km - 42.2m) < .1m) type = "MARATHON";
        string formatted = amount.ToString("00.0000", CultureInfo.InvariantCulture).Insert(5, ".");
        return new eventgetter.Distance { istance = formatted, unit = miles ? "mi" : "km" };
    }
}
