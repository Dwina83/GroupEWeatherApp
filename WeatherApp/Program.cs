using System.Text.RegularExpressions;

namespace WeatherApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string output = ReadWriteFile.ReadFile("Data.txt");
            string pattern = @"(?<year>2016)-(?<month>([0-9][^5]))-(?<date>\d{2}).(?<hour>\d{2}):(?<minute>\d{2}):(?<second>\d{2}),(?<location>Inne|Ute|ne|te),(?<temp>((\-\d{1,2})|(\d{1,2})).[\d]{1}),(?<humidity>\d{2})";
            Regex regex = new Regex(pattern); // @"(?<year>2016)-(?<month>([0-9][^5]))-(?<date>\d{2}).(?<hour>\d{2}):(?<minute>\d{2}):(?<second>\d{2}),(?<location>Inne|Ute|ne|te),(?<temp>((\-\d{1,2})|(\d{1,2})).[\d]{1}),(?<humidity>\d{2})"

            MatchCollection matches = Regex.Matches(output, pattern);
            int test = 0;



            Outdoor.DaysSortedByTemp(matches);

            // Outdoor.AverageTemp(matches);

        }
    }
}
