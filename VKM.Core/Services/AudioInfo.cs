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

        public string Pack()
        {
            return source + ";" + author + ";" + name + ";" + duration.ToString();
        }

        public static AudioInfo UnPack(string packedLine)
        {
            string[] splitedValues = packedLine.Split(';');
            return new AudioInfo()
            {
                source = splitedValues[0],
                author = splitedValues[1],
                name = splitedValues[2],
                duration = Convert.ToInt32(splitedValues[3])
            };
        }
    }
}
