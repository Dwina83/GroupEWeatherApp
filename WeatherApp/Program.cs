using System.Diagnostics;
using System.Text.RegularExpressions;

namespace WeatherApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ReadWriteFile.ClearFile();

            string output = ReadWriteFile.ReadFile("Data.txt");
            string pattern = @"(?<year>2016)-(?<month>([0-9][^5]))-(?<date>\d{2}).(?<hour>\d{2}):(?<minute>\d{2}):(?<second>\d{2}),(?<location>Inne|Ute|ne|te),(?<temp>((\-\d{1,2})|(\d{1,2})).[\d]{1}),(?<humidity>\d{2})";
            Regex regex = new Regex(pattern); // @"(?<year>2016)-(?<month>([0-9][^5]))-(?<date>\d{2}).(?<hour>\d{2}):(?<minute>\d{2}):(?<second>\d{2}),(?<location>Inne|Ute|ne|te),(?<temp>((\-\d{1,2})|(\d{1,2})).[\d]{1}),(?<humidity>\d{2})"

            MatchCollection matches = Regex.Matches(output, pattern);

            var avgDaysOutside = Helpers.CreateListOfDaysWithValues(matches, "Ute");
            Helpers.MoldIndex(avgDaysOutside);
            ReadWriteFile.WriteFile("Ute");
            Helpers.WriteAverageMonthly(avgDaysOutside);

            
            var avgDaysInside = Helpers.CreateListOfDaysWithValues(matches, "Inne");
            Helpers.MoldIndex(avgDaysInside);
            ReadWriteFile.WriteFile("Inne");
            Helpers.WriteAverageMonthly(avgDaysInside);


            Helpers.AverageTemp(matches, avgDaysOutside);

            Helpers.IsMeterologic(avgDaysOutside, "Winter");
            Helpers.IsMeterologic(avgDaysOutside, "Autumn");

            ReadWriteFile.WriteFile("Mögelformel ((day.Humidity - 78) * (day.Temp / 15)) / 0.22");

            var avgDaysOutsideTemp1 = avgDaysOutside.OrderByDescending(x => x.Temp); // Hottest to coldest day.
            var avgDaysOutsideTemp2 = avgDaysOutside.OrderBy(x => x.Humidity); // Dryest to the most humid day.
            var avgDaysOutsideTemp3 = avgDaysOutside.OrderBy(x => x.MoldIndex); // Least to greatest risk of mold.

            //foreach (var day in avgDaysOutsideTemp1)
            //{
            //    Console.WriteLine("dag: " + day.Date + " månad: " + day.Month + " värde: " + day.Temp);
            //}
        }
    }
}
