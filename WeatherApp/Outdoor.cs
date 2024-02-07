using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WeatherApp.Queries;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WeatherApp
{
    internal class Outdoor
    {
        public static void AverageTemp(MatchCollection matches)
        {
            Console.Write("Enter start period (month-date): ");
            string input = Console.ReadLine();
            Console.Write("Enter end period (month-date): ");
            string input2 = Console.ReadLine();

            string pattern = @"(?<month>[0-1][0-9])-(?<date>[0-3][0-9])";

            Regex regex = new Regex(pattern);

            Match match1 = regex.Match(input);
            int month1 = -1;
            int date1 = -1;
            Match match2 = regex.Match(input2);
            int month2 = -1;
            int date2 = -1;

            try
            {
                if (match1.Success)
                {
                    Console.WriteLine("first match!");
                    month1 = int.Parse(match1.Groups["month"].Value);
                    date1 = int.Parse((match1.Groups["date"].Value));
                }
                if (match2.Success)
                {
                    Console.WriteLine("second match!");
                    month2 = int.Parse(match2.Groups["month"].Value);
                    date2 = int.Parse(match2.Groups["date"].Value);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("invalid input!");

            }

            double avgTemp = 0;
            double avgHumidity = 0;
            int days = 0;


            foreach (Match match in matches)
            {
                int month = int.Parse(match.Groups["month"].Value);
                int date = int.Parse(match.Groups["date"].Value);
                if (month >= month1 && month <= month2 && date >= date1 && date <= date2 && (match.Groups["location"].Value == "Ute" || match.Groups["location"].Value == "te"))
                {
                    Console.WriteLine(match.Value + " success!");
                    avgTemp += double.Parse(match.Groups["temp"].Value, CultureInfo.InvariantCulture);
                    avgHumidity += double.Parse(match.Groups["humidity"].Value, CultureInfo.InvariantCulture);
                    days++;
                }
            }

            Console.WriteLine("Medeltemperatur = " + Math.Round(avgTemp/days, 2));
            Console.WriteLine("Medel luftfuktighet = " + Math.Round(avgHumidity/days, 2));
        }

        public static void DaysSortedByTemp(MatchCollection matches, string location, string type)
        {
            
            IEnumerable<Match> relevantMatches = matches.Cast<Match>()
                .Where(match => match.Success &&
                            (match.Groups["location"].Value == location));

            List<AvgTemp> sortedDays = new List<AvgTemp> ();
            foreach (Match match in relevantMatches)
            {
                int month = int.Parse(match.Groups["month"].Value);
                int date = int.Parse(match.Groups["date"].Value);

                bool exists = sortedDays.Any(day => day.Month == month && day.Date == date);

                if (!exists)
                {
                    sortedDays.Add(new AvgTemp { Month = month, Date = date });
                }
            }

            

            if (type == "temp")
            {
                int iterator = 1;
                foreach (var day in sortedDays)
                {
                    day.Temp = relevantMatches.Where(x => int.Parse(x.Groups["month"].Value) == day.Month && int.Parse(x.Groups["date"].Value) == day.Date).Average(x => double.Parse(x.Groups[type].Value, CultureInfo.InvariantCulture));
                }

                sortedDays = sortedDays.OrderByDescending(x => x.Temp).ToList();

                foreach (var day in sortedDays)
                {
                    Console.WriteLine(iterator + $" Dag: " + day.Date + " Månad: " + day.Month + " " + type + ": " + Math.Round(day.Temp, 2));
                    iterator++;
                }
            }
            else
            {
                int iterator = 1;
                foreach (var day in sortedDays)
                {
                    day.Humidity = relevantMatches.Where(x => int.Parse(x.Groups["month"].Value) == day.Month && int.Parse(x.Groups["date"].Value) == day.Date).Average(x => double.Parse(x.Groups[type].Value, CultureInfo.InvariantCulture));
                }

                sortedDays = sortedDays.OrderBy(x => x.Humidity).ToList();

                foreach (var day in sortedDays)
                {
                    Console.WriteLine(iterator +  $" Dag: " + day.Date + " Månad: " + day.Month + " " + type + ": " + Math.Round(day.Humidity, 2));
                    iterator++;
                }
            }
            

        }
    }
}
