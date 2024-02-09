using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WeatherApp.Queries;


namespace WeatherApp
{
    internal class Indoor
    {
        public static void MoldIndex(List<AvgTemp> avgDays)
        {
            foreach (var day in avgDays)
            {
                day.MoldIndex = day.MoldIndex = ((day.Humidity - 78) * (day.Temp / 15)) / 0.22;
            }
        }


    }
}
