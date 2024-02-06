using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp
{
    internal class ReadWriteFile
    {
        public static string path = "../../../Files/";
        public static string ReadFile(string fileName) 
        {
            string line = "";
            using (StreamReader reader = new StreamReader(path + fileName)) 
            { 
                line = reader.ReadToEnd();

            }
            return line;
        }
        
    }
}
