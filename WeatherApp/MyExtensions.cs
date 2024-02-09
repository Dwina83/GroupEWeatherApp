using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WeatherApp.Queries;

namespace WeatherApp
{
    public static class MyExtensions
    {
        public static IEnumerable<Match> FilterByLocation(this IEnumerable<Match> matches, string location)
        {
            // Fungerar inte timed out
            IEnumerable<Match> relevantMatches = matches
                .Where(match => match.Success &&
                            (match.Groups["location"].Value == location));


            return relevantMatches;
        }

        public static List<AvgTemp> CreateListOfDays(this IEnumerable<Match> matches)
        {
            List<AvgTemp> sortedDays = new List<AvgTemp>();
            foreach (Match match in matches)
            {
                int month = int.Parse(match.Groups["month"].Value);
                int date = int.Parse(match.Groups["date"].Value);

                bool exists = sortedDays.Any(day => day.Month == month && day.Date == date);

                if (!exists)
                {
                    sortedDays.Add(new AvgTemp { Month = month, Date = date });
                }
            }

            return sortedDays;
        }

        public static List<AvgTemp> AssignValues(this List<AvgTemp> days, IEnumerable<Match> matches, string location)
        {
           // Retrive matches with correct location and group by month, date
            var groupedMatches = matches.Where(x => x.Groups["location"].Value == location)
                .GroupBy(
                x => new { Month = int.Parse(x.Groups["month"].Value), Date = int.Parse(x.Groups["date"].Value) }
                );

            //var days1 = days;
            //var days2 = days;
            var days3 = days;

            Stopwatch sw = Stopwatch.StartNew();
            //foreach (var day in days1)
            //{
            //    day.Temp = matches.Where(x => int.Parse(x.Groups["month"].Value) == day.Month && int.Parse(x.Groups["date"].Value) == day.Date).Average(x => double.Parse(x.Groups["temp"].Value, CultureInfo.InvariantCulture));
            //    day.Humidity = matches.Where(x => int.Parse(x.Groups["month"].Value) == day.Month && int.Parse(x.Groups["date"].Value) == day.Date).Average(x => double.Parse(x.Groups["humidity"].Value, CultureInfo.InvariantCulture));
            //}
            //Console.WriteLine("Gamla LINQ: " + sw.ElapsedMilliseconds);

            //sw.Restart();
            //foreach (var day in days2)
            //{
            //    var relevantMatches = groupedMatches.FirstOrDefault(
            //        group => group.Key.Month == day.Month && group.Key.Date == day.Date);

            //    if (relevantMatches != null)
            //    {
            //        day.Temp = relevantMatches.Average(x => double.Parse(x.Groups["temp"].Value, CultureInfo.InvariantCulture));
            //        day.Humidity = relevantMatches.Average(x => double.Parse(x.Groups["humidity"].Value, CultureInfo.InvariantCulture));
            //    }
            //}
            //Console.WriteLine("Ny LINQ: " + sw.ElapsedMilliseconds);

            //sw.Restart();

            // Sort groups by month, date
            var orderedGroupedMatches = groupedMatches.OrderBy(group => group.Key.Month)
                                                        .ThenBy(group => group.Key.Date);
            Dictionary<(int, int), List<double>> temps = new();
            Dictionary<(int, int), List<double>> humidity = new();
            foreach (var group in orderedGroupedMatches)
            {
                List<double> tempsToAdd = new();
                List<double> humidityToAdd = new();

                foreach (var date in group)
                {
                    // Add temps to list of specific date
                    tempsToAdd.Add(double.Parse(date.Groups["temp"].Value, CultureInfo.InvariantCulture));
                    humidityToAdd.Add(double.Parse(date.Groups["humidity"].Value, CultureInfo.InvariantCulture));
                }
                var dateToAdd = group.Key.Date;
                var monthToAdd = group.Key.Month;
                // Add list of specific date to temp/humidity dict.
                temps.Add((dateToAdd, monthToAdd), tempsToAdd);
                humidity.Add((dateToAdd, monthToAdd), humidityToAdd);
            }
            foreach (var day in days3)
            {
                foreach (var date in temps)
                {
                    if (date.Key == (day.Date, day.Month))
                    {
                        // Average all temps in dict list of specific date and add to the specific days temp in day list 
                        day.Temp = date.Value.Average();
                        break;
                    }
                }
                foreach (var date in humidity)
                {
                    if (date.Key == (day.Date, day.Month))
                    {
                        // Average all humidity in dict list of specific date and add to the specific days humidity in day list 
                        day.Humidity = date.Value.Average();
                        break;
                    }
                }
            }
            Console.WriteLine("Simons metod: " + sw.ElapsedMilliseconds); // Har inte test tillräckligt mycket för att säga att den gör vad den ska men får ut rimliga värden.
            sw.Stop();

            //if (days1.Equals(days2) && days1.Equals(days3) && days2.Equals(days3)) // Verkar vara korrekt
            //{
            //    Console.WriteLine("korrekt");
            //}
           
            return days;
        }

       

    }
}
