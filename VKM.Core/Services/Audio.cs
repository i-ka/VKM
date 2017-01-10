using MvvmCross.Core.ViewModels;

namespace VKM.Core.Services
{
    public class Audio : MvxViewModel
    {
        private bool _isPlaying;

        public Audio(int id, string author, string name, int duration, string source = "")
        {
            Id = id;
            Author = author;
            Name = name;
            Duration = duration;
            Source = source;
            IsPlaying = false;
        }

        public AudioInfo AudioInfo { get; } = new AudioInfo();

        public int Id
        {
            get { return AudioInfo.id; }
            set
            {
                AudioInfo.id = value;
                RaisePropertyChanged(() => Id);
            }
        }

        public string Source
        {
            get { return AudioInfo.source; }
            private set
            {
                AudioInfo.source = value;
                RaisePropertyChanged(() => Source);
            }
        }

        public string Author
        {
            get { return AudioInfo.author; }
            private set
            {
                AudioInfo.author = value;
                RaisePropertyChanged(() => Author);
            }
        }

        public string Name
        {
            get { return AudioInfo.name; }
            set
            {
                AudioInfo.name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        public int Duration
        {
            get { return AudioInfo.duration; }
            private set
            {
                AudioInfo.duration = value;
                RaisePropertyChanged(() => Duration);
            }
        }

        public bool IsPlaying
        {
            get { return _isPlaying; }
            set
            {
                _isPlaying = value;
                RaisePropertyChanged(() => IsPlaying);
            }
        }
    }
}