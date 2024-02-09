using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WeatherApp.Queries;


namespace WeatherApp
{
    internal class Indoor
    {
        public static void MoldIndex(List<AvgTemp> avgDays)
        {
            // var relevantMatches2 = Helpers.FilterByLocation(matches, location);

            //IEnumerable<Match> relevantMatches = matches.Cast<Match>()
            //    .Where(match => match.Success &&
            //                (match.Groups["location"].Value == location));

            //var sortedMatches = relevantMatches
            //    .Where(x => double.Parse(x.Groups["temp"].Value, CultureInfo.InvariantCulture) >= 0)
            //    .OrderBy(x => (double.Parse(x.Groups["humidity"].Value) - 78) * (double.Parse(x.Groups["temp"].Value, CultureInfo.InvariantCulture) / 15) / 0.22);

            //foreach (var match in sortedMatches)
            //{
            //    Console.WriteLine("Luftfuktighet: " + match.Groups["humidity"].Value + " Temperatur: " + match.Groups["temp"].Value, CultureInfo.InvariantCulture);
            //}


            
            foreach (var day in avgDays)
            {
                day.MoldIndex = Math.Round(Math.Min(100, Math.Max(0, (day.Humidity - 78.0 * day.Temp) / 15.0 / 0.22) / 20), 2);
            }

            int test = 0;
        }


    }
}
