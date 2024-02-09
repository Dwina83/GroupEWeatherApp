using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WeatherApp.Queries;

namespace WeatherApp
{
    internal class Simons
    {
        public static List<AvgTemp> CreateListOfDaysWithValues(IEnumerable<Match> matches, string location)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            var filteredMatches = matches.FilterByLocation(location);
            Console.WriteLine("filtered matches: " + stopwatch.ElapsedMilliseconds);

            stopwatch.Restart();
            var days = filteredMatches.CreateListOfDays();
            Console.WriteLine("CreateListOfDays: " + stopwatch.ElapsedMilliseconds);

            stopwatch.Restart();
            var avgList = days.AssignValues(matches, location);
            Console.WriteLine("AssignValues: " + stopwatch.ElapsedMilliseconds);
            stopwatch.Stop();
            return avgList;
        }
    }
}
