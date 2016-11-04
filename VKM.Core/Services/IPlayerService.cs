using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKM.Core.Services
{
    public enum PlaybackState
    {
        Playing,
        Stoped,
        Paused,
        EndOfPlayback,
        NoMedia
    }
    public interface IPlayerService
    {
        string Source { get; set; }
        Task Play();
        void Pause();
        void Stop();
        void Seek(int pos);
    }
}
