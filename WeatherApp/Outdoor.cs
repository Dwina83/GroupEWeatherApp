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

            Console.WriteLine("Medeltemperatur = " + Math.Round(avgTemp / days, 2));
            Console.WriteLine("Medel luftfuktighet = " + Math.Round(avgHumidity / days, 2));
        }

        public static void DaysSortedByTemp(MatchCollection matches, string location, string type)
        {

            IEnumerable<Match> relevantMatches = matches.Cast<Match>()
                .Where(match => match.Success &&
                            (match.Groups["location"].Value == location));

            List<AvgTemp> sortedDays = new List<AvgTemp>();
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
                    Console.WriteLine(iterator + $" Dag: " + day.Date + " Månad: " + day.Month + " " + type + ": " + Math.Round(day.Humidity, 2));
                    iterator++;
                }
            }
        }

        public static void DailyTemp(MatchCollection matches, string location)
        {
            IEnumerable<Match> relevantMatches = matches.Cast<Match>()
                .Where(match => match.Success &&
                            (match.Groups["location"].Value == location));

            List<AvgTemp> sortedDays = new List<AvgTemp>();
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

        }

        public static void IsMeterologic(MatchCollection matches, string season)
        {

            IEnumerable<Match> relevantMatches = matches.Cast<Match>()
                .Where(match => match.Success &&
                            (match.Groups["location"].Value == "Ute"));

            List<AvgTemp> sortedDays = new List<AvgTemp>();
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


            foreach (var day in sortedDays)
            {
                day.Temp = relevantMatches.Where(x => int.Parse(x.Groups["month"].Value) == day.Month && int.Parse(x.Groups["date"].Value) == day.Date).Average(x => double.Parse(x.Groups["temp"].Value, CultureInfo.InvariantCulture));
            }

            sortedDays = sortedDays.OrderBy(x => x.Month).ToList();

            if (season == "Winter")
            {



                List<AvgTemp> coldestDays = new List<AvgTemp>();
                List<AvgTemp> coldestDaysTemp = new List<AvgTemp>();
                foreach (var day in sortedDays)
                {
                    coldestDaysTemp.Add(day);
                    if (coldestDaysTemp.Count > 5)
                    {
                        coldestDaysTemp = coldestDaysTemp.GetRange(1, 5);
                    }
                    double sum1 = coldestDaysTemp.Select(x => x.Temp).Sum();
                    double sum2 = coldestDays.Select(x => x.Temp).Sum();
                    if (coldestDaysTemp.Count == 5 && (coldestDays.Count < 5 || coldestDaysTemp.Select(x => x.Temp).Sum() < coldestDays.Select(x => x.Temp).Sum()))
                    {
                        coldestDays = new(coldestDaysTemp);

                    }
                }

                Console.WriteLine("Starten på den kallaste perioden på vintern är dag: " + coldestDays[0].Date + " månad " + coldestDays[0].Month);

                ReadWriteFile.WriteFile("Starten på den kallaste perioden på vintern är dag: " + coldestDays[0].Date + " månad " + coldestDays[0].Month);

            }

            if (season == "Autumn")
            {
                int iterator = 0;
                int firstDate = 0;
                int firstMonth = 0;

                foreach (var day in sortedDays)
                {
                    if (day.Temp < 10)
                    {
                        if (iterator == 0)
                        {
                            firstDate = day.Date;
                            firstMonth = day.Month;
                        }
                        iterator++;
                        if (iterator == 5 && day.Temp < 10)
                        {
                            if (firstMonth > 7 || firstMonth == 1 || (firstMonth == 2 && firstDate < 14))
                            {
                                Console.WriteLine("Dag: " + firstDate + " Månad: " + firstMonth + " är metrologisk höst.");
                                ReadWriteFile.WriteFile("Dag: " + firstDate + " Månad: " + firstMonth + " är metrologisk höst.");
                                break;
                            }
                        }
                    }
                    else
                    {
                        iterator = 0;
                    }
                }
            }
        }

        public static void WriteAverageMonthly(MatchCollection matches, string location)
        {
            IEnumerable<Match> relevantMatches = matches.Cast<Match>()
                .Where(match => match.Success &&
                            (match.Groups["location"].Value == location));

            List<AvgTemp> sortedDays = new List<AvgTemp>();
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

            foreach (var day in sortedDays)
            {
                day.Temp = relevantMatches.Where(x => int.Parse(x.Groups["month"].Value) == day.Month && int.Parse(x.Groups["date"].Value) == day.Date).Average(x => double.Parse(x.Groups["temp"].Value, CultureInfo.InvariantCulture));
            }


            Dictionary<int, string> months = new Dictionary<int, string>();
            months.Add(6, "juni");
            months.Add(7, "juli");
            months.Add(8, "augusti");
            months.Add(9, "september");
            months.Add(10, "oktober");
            months.Add(11, "november");
            months.Add(12, "december");

            int currentMonth = 6;
            double sum = 0;
            int days = 0;
            ReadWriteFile.WriteFile("Genomsnittstemperatur per månad.");
            foreach (var day in sortedDays)
            {
                sum += day.Temp;
                days++;

                if (day.Month != currentMonth || sortedDays.Last() == day)
                {
                    double avg = sum/ days;
                    ReadWriteFile.WriteFile(months[currentMonth] + ": " + Math.Round(avg, 2));
                    currentMonth = day.Month;
                    sum = 0;
                    days = 0;
                }
            }
        }
    }
}
