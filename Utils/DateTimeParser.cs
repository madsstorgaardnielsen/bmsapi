using System.Globalization;

namespace BMSAPI.Utils;

public static class DateTimeParser {
    public static string[] ToString(DateTime date) {
        var s = date.ToString();
        var splitted = s.Split("T");
        var splitDate = splitted[0].Split("-");
        var formattedDate = splitDate[2] + "-" + splitted[1] + "-" + splitDate[0];
        var splitTime = splitted[1].Split(":");
        var formattedTime = splitTime[0] + ":" + splitTime[1];

        return new[] {formattedDate, formattedTime};
    }

    public static DateTime ToDateTime(string date) {

        var s = date.Split("-");
        var dateString = s[2] + "-" + s[1] + "-" + s[0];

        Console.WriteLine("DATE "+date);
        Console.WriteLine("PARSED DATE: "+DateTime.Parse(dateString));
        return DateTime.Parse(dateString);
    }
}