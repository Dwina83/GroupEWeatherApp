using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WeatherApp
{
    internal class Simons
    {
        public static void DaysSortedByTemp2(IEnumerable<Match> matches, string location)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            var filteredMatches = matches.FilterByLocation(location);
            Console.WriteLine("filtered matches: " + stopwatch.ElapsedMilliseconds);

            stopwatch.Restart();
            var days = filteredMatches.CreateListOfDays();
            Console.WriteLine("CreateListOfDays: " + stopwatch.ElapsedMilliseconds);

            stopwatch.Restart();
            var avgList = days.AssignValues(matches, "Ute");
            foreach (var day in avgList)
            {
                Console.WriteLine("dag: " + day.Date + " månad: " + day.Month + " temp: " + day.Temp + " humidity: " + day.Humidity);

            }
            Console.WriteLine("AssignValues: " + stopwatch.ElapsedMilliseconds);
            int test = 0;
        }
    }
}
