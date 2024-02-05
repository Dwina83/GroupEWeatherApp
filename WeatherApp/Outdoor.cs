using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WeatherApp
{
    internal class Outdoor
    {
        public static void AverageTemp(List<string> data)
        {
            Console.Write("Enter start period (month-date): ");
            string input = Console.ReadLine();
            Console.Write("Enter end period (month-date): ");
            string input2 = Console.ReadLine();

            string pattern = @"(?<month>[0-1][0-9])-(?<date>[0-3][0-9])";

            Regex regex = new Regex(pattern);

            Match match = regex.Match(input);
            Match match2 = regex.Match(input2);
            try
            {
                if (match.Success)
                {
                    Console.WriteLine("first match!");
                }
                if (match2.Success)
                {
                    Console.WriteLine("second match!");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("invalid input!");
            }




        }
    }
}
