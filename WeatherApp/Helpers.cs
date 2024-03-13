using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WeatherApp
{
    internal class Helpers
    {
        public static void AverageTemp(MatchCollection matches, List<AvgTemp> days)
        {
            Console.Write("Start datum (månad-dag): ");
            string input = Console.ReadLine();
            Console.Write("Slut datum (månad-dag): ");
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
                    Console.WriteLine("Korrekt start datum");
                    month1 = int.Parse(match1.Groups["month"].Value);
                    date1 = int.Parse((match1.Groups["date"].Value));
                }
                if (match2.Success)
                {
                    Console.WriteLine("Korrekt slut datum");
                    month2 = int.Parse(match2.Groups["month"].Value);
                    date2 = int.Parse(match2.Groups["date"].Value);
                }

                Console.WriteLine("Medeltemperatur = " + Math.Round((days.Where(x => x.Month >= month1 && x.Month <= month2 && x.Date >= date1 && x.Date <= date2).Average(x => x.Temp)), 2) + " grader");
                Console.WriteLine("Medel luftfuktighet = " + Math.Round(days.Where(x => x.Month >= month1 && x.Month <= month2 && x.Date >= date1 && x.Date <= date2).Average(x => x.Humidity)), 2);
                Console.WriteLine("------------------------------------------------------------------------------------------------------");

            }
            catch (Exception ex)
            {
                Console.WriteLine("invalid input!");
            }


        }

        // Obsolete
        //public static void DaysSortedByTemp(MatchCollection matches, string location, string type)
        //{

        //    IEnumerable<Match> relevantMatches = matches.Cast<Match>()
        //        .Where(match => match.Success &&
        //                    (match.Groups["location"].Value == location));

        //    List<AvgTemp> sortedDays = new List<AvgTemp>();
        //    foreach (Match match in relevantMatches)
        //    {
        //        int month = int.Parse(match.Groups["month"].Value);
        //        int date = int.Parse(match.Groups["date"].Value);

        //        bool exists = sortedDays.Any(day => day.Month == month && day.Date == date);

        //        if (!exists)
        //        {
        //            sortedDays.Add(new AvgTemp { Month = month, Date = date });
        //        }
        //    }

        //    if (type == "temp")
        //    {
        //        int iterator = 1;
        //        foreach (var day in sortedDays)
        //        {
        //            day.Temp = relevantMatches.Where(x => int.Parse(x.Groups["month"].Value) == day.Month && int.Parse(x.Groups["date"].Value) == day.Date).Average(x => double.Parse(x.Groups[type].Value, CultureInfo.InvariantCulture));
        //        }


        //        sortedDays = sortedDays.OrderByDescending(x => x.Temp).ToList();

        //        foreach (var day in sortedDays)
        //        {
        //            Console.WriteLine(iterator + $" Dag: " + day.Date + " Månad: " + day.Month + " " + type + ": " + Math.Round(day.Temp, 2));
        //            iterator++;
        //        }

        //    }
        //    else
        //    {
        //        int iterator = 1;
        //        foreach (var day in sortedDays)
        //        {
        //            day.Humidity = relevantMatches.Where(x => int.Parse(x.Groups["month"].Value) == day.Month && int.Parse(x.Groups["date"].Value) == day.Date).Average(x => double.Parse(x.Groups[type].Value, CultureInfo.InvariantCulture));
        //        }

        //        sortedDays = sortedDays.OrderBy(x => x.Humidity).ToList();

        //        foreach (var day in sortedDays)
        //        {
        //            Console.WriteLine(iterator + $" Dag: " + day.Date + " Månad: " + day.Month + " " + type + ": " + Math.Round(day.Humidity, 2));
        //            iterator++;
        //        }
        //    }
        //} 


        public static void IsMeterologic(List<AvgTemp> sortedDays, string season)
        {
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
                                Console.WriteLine("Dag: " + firstDate + " Månad: " + firstMonth + " är meteorologisk höst.");
                                ReadWriteFile.WriteFile("Dag: " + firstDate + " Månad: " + firstMonth + " är meteorologisk höst.");
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

        public static void WriteAverageMonthly(List<AvgTemp> dayList)
        {
            Dictionary<int, string> months = new Dictionary<int, string>();
            months.Add(6, "juni");
            months.Add(7, "juli");
            months.Add(8, "augusti");
            months.Add(9, "september");
            months.Add(10, "oktober");
            months.Add(11, "november");
            months.Add(12, "december");

            for (int i = 6; i < 13; i++)
            {
                string toPrint = months[i] + ": " + "Temperatur: " + Math.Round(dayList.Where(day => day.Month == i).Average(day => day.Temp), 2) + " Luftfuktighet: " + Math.Round(dayList.Where(day => day.Month == i).Average(day => day.Humidity), 2) + " Mögelindex: " + Math.Round(dayList.Where(day => day.Month == i).Average(day => day.MoldIndex), 2);
                ReadWriteFile.WriteFile(toPrint);
            }
        }

        public static List<AvgTemp> CreateListOfDaysWithValues(IEnumerable<Match> matches, string location)
        {
            var filteredMatches = matches.FilterByLocation(location);
            var days = filteredMatches.CreateListOfDays();
            var avgList = days.AssignValues(matches, location);
            return avgList;
        }

        public static List<AvgTemp> MoldIndex(List<AvgTemp> avgDays)
        {
            foreach (var day in avgDays)
            {
                day.MoldIndex = ((day.Humidity - 78) * (day.Temp / 15)) / 0.22;
            }

            return avgDays;
        }
    }
}
