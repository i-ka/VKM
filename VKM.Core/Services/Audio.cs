using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;

namespace VKM.Core.Services
{
    public class Audio : MvxViewModel
    {
        public Audio(string author, string name, int duration)
        {
            Author = author;
            Name = name;
            Duration = duration;
            IsPlaying = false;
        }
        public string Author { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }
        private bool _isPlaying;
        public bool IsPlaying {
            get { return _isPlaying; }
            set
            {
                _isPlaying = value;
                RaisePropertyChanged(() => IsPlaying);
            }
        }
        public string Source { get; set; }
    }
}
