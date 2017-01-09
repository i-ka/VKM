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
        private AudioInfo _audioInfo = new AudioInfo();
        public Audio(int id, string author, string name, int duration, string source = "")
        {
            Id = id;
            Author = author;
            Name = name;
            Duration = duration;
            Source = source;
            IsPlaying = false;
        }
        public AudioInfo AudioInfo { get { return _audioInfo; } }

        public int Id
        {
            get { return _audioInfo.id; }
            set
            {
                _audioInfo.id = value;
                RaisePropertyChanged(() => Id);
            }
        }

        public string Source {
            get
            {
               return _audioInfo.source;
            }
            private set
            {
                _audioInfo.source = value;
                RaisePropertyChanged(() => Source);
            }
        }
        public string Author {
            get
            {
                return _audioInfo.author;
            }
            private set
            {
                _audioInfo.author = value;
                RaisePropertyChanged(() => Author);
            }
        }
        public string Name {
            get
            {
                return _audioInfo.name;
            }
            set
            {
                _audioInfo.name = value;
                RaisePropertyChanged(() => Name);
            }
        }
        public int Duration {
            get
            {
                return _audioInfo.duration;
            }
            private set
            {
                _audioInfo.duration = value;
                RaisePropertyChanged(() => Duration);
            }
        }
        private bool _isPlaying;
        public bool IsPlaying {
            get { return _isPlaying; }
            set
            {
                _isPlaying = value;
                RaisePropertyChanged(() => IsPlaying);
            }
        }
    }
}
