using System.Text.RegularExpressions;

namespace WeatherApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string output = ReadWriteFile.ReadFile("Data.txt");
            string pattern = @"(?<year>2016)-(?<month>([0-9][^5]))-(?<date>\d{2}).(?<hour>\d{2}):(?<minute>\d{2}):(?<second>\d{2}),(?<location>Inne|Ute|ne|te),(?<temp>((\-\d{1,2})|(\d{1,2})).[\d]{1}),(?<humidity>\d{2})";
            //Regex regex = new Regex(@"(?<year>2016)-(?<month>([0-9][^5]))-(?<date>\d{2}).(?<hour>\d{2}):(?<minute>\d{2}):(?<second>\d{2}),(?<location>Inne|Ute|ne|te),(?<temp>((\-\d{1,2})|(\d{1,2})).[\d]{1}),(?<humidity>\d{2})");

            MatchCollection matches = Regex.Matches(output, pattern);
            int test = 0;
            //foreach (string line in output)
            //{
            //    Match match = regex.Match(line);
            //    if (match.Success)
            //    {
            //        newOutput.Add(match.Value);
            //    }
            //    else
            //    {
            //        Console.WriteLine(line + " failed");
            //    }

            //}


            Outdoor.AverageTemp(output);

        }
    }
}
