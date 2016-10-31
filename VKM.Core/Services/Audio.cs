using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKM.Core.Services
{
    public class Audio
    {
        public Audio(string author, string name, int duration)
        {
            Author = author;
            Name = name;
            Duration = duration;
        }
        public string Author { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }
    }
}
