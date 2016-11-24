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
        Preparing,
        NoMedia
    }
    public interface IPlayerService
    {
        VkmPlaybackState Status{ get; }
        void Start();
        void Stop();
        void SetSource(Audio audio);
        void Pause();
    }
}