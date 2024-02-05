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
            //List<string> lines = new List<string>();
            string line = "";
            using (StreamReader reader = new StreamReader(path + fileName)) 
            { 
                line = reader.ReadToEnd();
                //while (line != null) 
                //{ 
                    
                //    lines.Add(line);
                //    line = reader.ReadLine();
                //}
            }
            return line;
        }
        
    }
}
