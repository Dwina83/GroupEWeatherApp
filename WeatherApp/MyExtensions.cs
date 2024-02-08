using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WeatherApp
{
    public static class MyExtensions
    {
        public static IEnumerable<Match> FilterByLocation(this IEnumerable<Match> matches, string location)
        {
            // Fungerar inte timed out
            IEnumerable<Match> relevantMatches = matches.Cast<Match>()
                .Where(match => match.Success &&
                            (match.Groups["location"].Value == location));


            return relevantMatches;
        }
    }
}
