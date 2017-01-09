using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKM.Core.Services
{
    public class AudioInfo
    {
        public string source { get; set; }
        public string author { get; set; }
        public string name { get; set; }
        public int duration { get; set; }
        public int id { get; set; }

        public string Pack()
        {
            return id.ToString() + ";" + source + ";" + author + ";" + name + ";" + duration.ToString();
        }

        public static AudioInfo UnPack(string packedLine)
        {
            string[] splitedValues = packedLine.Split(';');
            return new AudioInfo()
            {
                id =  Convert.ToInt32(splitedValues[0]),
                source = splitedValues[1],
                author = splitedValues[2],
                name = splitedValues[3],
                duration = Convert.ToInt32(splitedValues[4])
            };
        }
    }
}
