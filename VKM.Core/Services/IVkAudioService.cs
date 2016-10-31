using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKM.Core.Services
{
    public interface IVkAudioService
    {
        List<Audio> GetAudioList();
    }
}
