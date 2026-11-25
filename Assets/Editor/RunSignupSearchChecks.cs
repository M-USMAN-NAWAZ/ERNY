using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class RunSignupSearchChecks
{
    static RunSignupSearchChecks()
    {
        EditorApplication.delayCall += () => {
            const string request = "Library/RunSignupSearchChecks.request";
            if (File.Exists(request)) { File.Delete(request); Validate(); }
        };
    }

    [MenuItem("Tools/Erny/Validate RunSignup Search Mapping")]
    public static void Validate()
    {
        var filters = new RaceDiscoveryController.SearchFilters {
            name = "5K", country = "US", unit = "mi", minDistance = "1", maxDistance = "10", fromMonth = "12", toMonth = "2"
        };
        var urls = RunSignupRaceSearch.BuildUrls(filters, 2, new DateTime(2026,11,23));
        Require(urls.Count == 3 && urls.All(u => u.Contains("start_date=2026-12-01") && u.Contains("end_date=2027-02-28") && u.Contains("page=2")), "Date range or paging mapping failed");
        Require(urls.All(u => !u.Contains("api_secret") && !u.Contains("password")), "Credentials must not be embedded");
        Require(urls.Any(u => u.Contains("min_distance=1.60934") && u.Contains("max_distance=16.09344")), "Miles to kilometers conversion failed");
        var rows = RunSignupRaceSearch.Parse("{\"races\":[{\"race\":{\"race_id\":1,\"name\":\"Race &amp; Run\",\"next_date\":\"12/16/2026\",\"address\":{\"city\":\"Ballwin\",\"state\":\"MO\"},\"events\":[{\"distance\":\"5K\"}]}}]}");
        Require(rows.Count == 1 && rows[0].name == "Race & Run" && rows[0].date == "Wed 12/16/26" && rows[0].distances == "5K", "Response mapping failed");
        var days = RunSignupRaceSearch.Parse(
            "{\"races\":[{\"race\":{\"race_id\":12,\"name\":\"Weekend Run\",\"next_date\":\"10/24/2026\",\"events\":[" +
            "{\"name\":\"10K\",\"distance\":\"10K\",\"start_time\":\"2026-10-24T23:15:00-05:00\"}," +
            "{\"name\":\"Half\",\"distance\":\"13.1 Miles\",\"start_time\":\"10/25/2026 9:00 AM\"}]}}]}");
        Require(days[0].entries.Length == 2 && days[0].entries[0].date == new DateTime(2026, 10, 24)
            && days[0].entries[1].date == new DateTime(2026, 10, 25), "Use the selected event's local race day");
        var imported = RunSignupRaceSearch.ImportDistance(days[0].entries[1].distance, out string type);
        Require(days[0].entries[0].startTime == "2026-10-24T23:15:00-05:00"
            && days[0].entries[1].startTime == "10/25/2026 9:00 AM", "Keep the original scheduled start time");
        Require(type == "HALFATHON" && imported.istance == "13.10.00" && imported.unit == "mi", "Race distance mapping failed");
        Require(RunSignupRaceSearch.Parse("{\"races\":[]}").Count == 0, "Empty results handling failed");
        bool rejected = false;
        try { RunSignupRaceSearch.Parse("{\"error\":\"invalid parameters\"}"); }
        catch (FormatException) { rejected = true; }
        Require(rejected, "API errors must not look like empty results");
        File.WriteAllText("Library/RunSignupSearchChecks.result", "PASS: search, registration URLs, race days, imported distance, and API errors");
        Debug.Log("RunSignup search mapping checks passed.");
    }

    private static void Require(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
}
