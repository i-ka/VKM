using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VKM.Core.Services
{
    public enum VkmPlaybackState
    {
        Playing,
        Stoped,
        Paused,
        EndOfPlayback,
        NoMedia
    }
    public interface IPlayerService
    {
        void Start();
        void Stop();
        void SetPlayList(List<Audio> playList);
        void SetSource(string newSource);
        void Next();
        void Prev();
        void Goto(int idx);
        void Pause();
    }
}