using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Queries
{
    public class AvgTemp
    {
        public double Temp { get; set; }
        public double Humidity { get; set; }    
        public int Date { get; set; }
        public int Month { get; set; }
        public double MoldIndex { get; set; }
    }
}
